using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using DevCampDemos.Models;
using System.IO;
using Microsoft.PowerBI.Api.Models.Credentials;

namespace DevCampDemos.Services {

  class PowerBiManager {

    static string accessToken = TokenManager.GetAccessTokenWithLocalCredentials();

    // static string accessToken = TokenManager.GetAccessTokenWithCredentialsFromKeyVault();

    // static string accessToken = TokenManager.GetAccessTokenWithManagedIdentity();

    private const string urlPowerBiServiceApiRoot = "https://api.powerbi.com/";
    static TokenCredentials tokenCredentials = new TokenCredentials(accessToken, "Bearer");
    static PowerBIClient pbiClient = new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);

    public static async Task<IList<Group>> GetWorkspaces() {

      // call user API - get all workspaces in which service principal is a member
      var workspaces = (await pbiClient.Groups.GetGroupsAsync()).Value;

      return workspaces;
    }

    public static async Task<IList<AdminGroup>> GetWorkspacesAsAdmin() {

      // call admin API - get all workspaces in current Azure AD tenant
      string workspaceFilter = "state eq 'Active' and type eq 'Workspace'";
      var workspaces = await pbiClient.Groups.GetGroupsAsAdminAsync(top: 100, filter: workspaceFilter);

      return workspaces.Value;

    }

    public static async Task<IList<AdminGroup>> GetWorkspacesAsAdminWithExpand() {

      string workspaceFilter = "state eq 'Active' and type eq 'Workspace'";
      string workspaceExpand = "users, reports, dashboards, datasets, dataflows";

      var workspaces = await pbiClient.Groups.GetGroupsAsAdminAsync(top: 100,
                                                         filter: workspaceFilter,
                                                         expand: workspaceExpand);
      return workspaces.Value;
    }

    public static async Task<WorkspaceInfoResponse> ScanWorkspaceAsAdmin(Guid WorkspaceId) {

      // create RequiredWorkspaces parameter object used to call PostWorkspaceInfo
      RequiredWorkspaces requiredWorkspaces = new RequiredWorkspaces {
        Workspaces = new List<Guid?>() { WorkspaceId }
      };

      // start asynchronous workspace scan job
      ScanRequest scanStatus = await pbiClient.WorkspaceInfo.PostWorkspaceInfoAsync(requiredWorkspaces,
                                                                                    getArtifactUsers: true,
                                                                                    lineage: true,
                                                                                    datasourceDetails: true,
                                                                                    datasetSchema: true,
                                                                                    datasetExpressions: true);

      // get ID of asynchronous workspace scan job
      Guid scanId = scanStatus.Id.Value;

      while (scanStatus.Status.Equals("NotStarted") || scanStatus.Status.Equals("Running")) {
        // take a secord or two before polling for success
        System.Threading.Thread.Sleep(1000);
        // continue to call GetScanStatus until job has completed
        scanStatus = await pbiClient.WorkspaceInfo.GetScanStatusAsync(scanId);
      }

      // get results after succesful scan
      if (scanStatus.Status.Equals("Succeeded")) {
        WorkspaceInfoResponse scanResult = await pbiClient.WorkspaceInfo.GetScanResultAsync(scanId);
        return scanResult;
      }

      // handle error that occurred during workspace scan
      if (scanStatus.Status.Equals("Failed")) {
        throw new ApplicationException("Workspace Scanning Error: " + scanStatus.Error);
      }

      return null;
    }

    public static async Task<EmbeddedViewModel> GetEmbeddedViewModel() {

      Guid ReportId = new Guid(Environment.GetEnvironmentVariable("EmbeddedReportId"));
      Guid WorkspaceId = new Guid(Environment.GetEnvironmentVariable("EmbeddedReportWorkspaceId"));

      var report = await pbiClient.Reports.GetReportInGroupAsync(WorkspaceId, ReportId);

      var datasetRequests = new List<GenerateTokenRequestV2Dataset>();
      datasetRequests.Add(new GenerateTokenRequestV2Dataset(report.DatasetId));

      var reportRequests = new List<GenerateTokenRequestV2Report>();
      reportRequests.Add(new GenerateTokenRequestV2Report(report.Id, allowEdit: false));

      GenerateTokenRequestV2 tokenRequest =
        new GenerateTokenRequestV2 {
          Datasets = datasetRequests,
          Reports = reportRequests
        };

      // call to Power BI Service API and pass GenerateTokenRequest object to generate embed token
      var EmbedTokenResult = await pbiClient.EmbedToken.GenerateTokenAsync(tokenRequest);

      return new EmbeddedViewModel {
        embedToken = EmbedTokenResult.Token,
        reportId = report.Id.ToString(),
        embedUrl = report.EmbedUrl
      };

    }

    private static List<ActivityEventEntity> activityEvents = new List<ActivityEventEntity>();

    public async static Task<List<ActivityEventEntity>> GetActivityEvents(DateTime date) {

      string dateString = date.ToString("yyyy-MM-dd");

      string startDateTime = "'" + dateString + "T00:00:00'";
      string endDateTime = "'" + dateString + "T23:59:59'";

      ActivityEventResponse response = await pbiClient.Admin.GetActivityEventsAsync(startDateTime, endDateTime);

      ProcessActivityResponse(response);

      while (response.ContinuationToken != null) {
        string formattedContinuationToken = $"'{WebUtility.UrlDecode(response.ContinuationToken)}'";
        response = await pbiClient.Admin.GetActivityEventsAsync(null, null, formattedContinuationToken, null);
        ProcessActivityResponse(response);
      }

      return activityEvents;

    }

    private static void ProcessActivityResponse(ActivityEventResponse response) {

      foreach (var activityEventEntity in response.ActivityEventEntities) {
        string activityEventEntityJson = JsonConvert.SerializeObject(activityEventEntity);
        ActivityEventEntity activityEvent = JsonConvert.DeserializeObject<ActivityEventEntity>(activityEventEntityJson);
        activityEvents.Add(activityEvent);
      }

    }

    public static string ConvertObjectToJson(object targetObject) {

      JsonSerializerSettings settings = new JsonSerializerSettings {
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented
      };

      return JsonConvert.SerializeObject(targetObject, settings);

    }

    private static async Task PublishPBIX(Guid WorkspaceId, Stream pbixStream, string ImportName) {

      var import = await pbiClient.Imports.PostImportWithFileAsyncInGroup(WorkspaceId,
                                                                          pbixStream,
                                                                          ImportName,
                                                                          ImportConflictHandlerMode.CreateOrOverwrite);

      while (import.ImportState != "Succeeded") {
        import = await pbiClient.Imports.GetImportInGroupAsync(WorkspaceId, import.Id);
      }

    }

    private static Dataset GetDataset(Guid WorkspaceId, string DatasetName) {
      var datasets = pbiClient.Datasets.GetDatasetsInGroup(WorkspaceId).Value;
      foreach (var dataset in datasets) {
        if (dataset.Name.Equals(DatasetName)) {
          return dataset;
        }
      }
      return null;
    }

    private static async Task PatchBlobStorageCredentials(Guid WorkspaceId, string DatasetId) {

      var datasources = (await pbiClient.Datasets.GetDatasourcesInGroupAsync(WorkspaceId, DatasetId)).Value;

      // find the target datasource
      foreach (var datasource in datasources) {
        if (datasource.DatasourceType.ToLower() == "azureblobs") {
          // get the datasourceId and the gatewayId
          var datasourceId = datasource.DatasourceId;
          var gatewayId = datasource.GatewayId;
          // Create UpdateDatasourceRequest to update Azure SQL datasource credentials
          UpdateDatasourceRequest req = new UpdateDatasourceRequest {
            CredentialDetails = new CredentialDetails(
              new KeyCredentials(Environment.GetEnvironmentVariable("BlobStorageKey")),
              PrivacyLevel.None,
              EncryptedConnection.NotEncrypted)
          };
          // Execute Patch command to update Azure SQL datasource credentials
          await pbiClient.Gateways.UpdateDatasourceAsync((Guid)gatewayId, (Guid)datasourceId, req);
        }
      };

    }

    public static async Task ProcessFileUpload(Guid WorkspaceId, Stream pbixStream, string FileName) {

      string ImportName = FileName.Replace(".csv", "");

      await PublishPBIX(WorkspaceId, pbixStream, ImportName);

      Dataset dataset = GetDataset(WorkspaceId, ImportName);

      UpdateMashupParametersRequest req =
        new UpdateMashupParametersRequest(new List<UpdateMashupParameterDetails>() {
          new UpdateMashupParameterDetails { Name = "CsvFileName", NewValue = FileName }
      });

      await pbiClient.Datasets.UpdateParametersInGroupAsync(WorkspaceId, dataset.Id, req);

      await PatchBlobStorageCredentials(WorkspaceId, dataset.Id);

      await pbiClient.Datasets.RefreshDatasetInGroupAsync(WorkspaceId, dataset.Id);

    }

  }

}
