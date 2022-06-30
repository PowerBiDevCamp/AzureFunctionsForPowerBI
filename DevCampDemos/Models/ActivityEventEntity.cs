using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevCampDemos.Models {

  class ActivityEventEntity {
    public string Id { get; set; }
    public int RecordType { get; set; }
    public DateTime CreationTime { get; set; }
    public string Operation { get; set; }
    public string OrganizationId { get; set; }
    public int UserType { get; set; }
    public string UserKey { get; set; }
    public string Workload { get; set; }
    public string UserId { get; set; }
    public string ClientIP { get; set; }
    public string UserAgent { get; set; }
    public string Activity { get; set; }
    public string ItemName { get; set; }
    public string ObjectId { get; set; }
    public string RequestId { get; set; }
    public string ActivityId { get; set; }
    public bool IsSuccess { get; set; }
    public string WorkSpaceName { get; set; }
    public string WorkspaceId { get; set; }
    public string ImportId { get; set; }
    public string ImportSource { get; set; }
    public string ImportType { get; set; }
    public string ImportDisplayName { get; set; }
    public string DatasetName { get; set; }
    public string DatasetId { get; set; }
    public string DataConnectivityMode { get; set; }
    public string GatewayId { get; set; }
    public string GatewayName { get; set; }
    public string GatewayType { get; set; }
    public string ReportName { get; set; }
    public string ReportId { get; set; }
    public string ReportType { get; set; }
    public string FolderObjectId { get; set; }
    public string FolderDisplayName { get; set; }
    public string ArtifactId { get; set; }
    public string ArtifactName { get; set; }
    public string CapacityName { get; set; }
    public string CapacityUsers { get; set; }
    public string CapacityState { get; set; }
    public string DistributionMethod { get; set; }
    public string ConsumptionMethod { get; set; }
    public string RefreshType { get; set; }
    public string ExportEventStartDateTimeParameter { get; set; }
    public string ExportEventEndDateTimeParameter { get; set; }

    public List<PowerBIActivityLogDataset> Datasets { get; set; }
    public List<PowerBIActivityLogSharingInformation> SharingInformation { get; set; }
    public List<PowerBIActivityLogDatasource> Datasources { get; set; }
    public List<PowerBIActivityLogSubscribeeInformation> SubscribeeInformation { get; set; }
    public PowerBIActivityLogExportedArtifactInfo ExportedArtifactInfo { get; set; }
    public PowerBIActivityLogAuditedArtifactInformation AuditedArtifactInformation { get; set; }

    public class PowerBIActivityLogDataset {
      public string DatasetId { get; set; }
      public string DatasetName { get; set; }
    }

    public class PowerBIActivityLogSharingInformation {
      public string RecipientEmail { get; set; }
      public string ResharePermission { get; set; }
    }

    public class PowerBIActivityLogDatasource {
      public string DatasourceType { get; set; }
      public string ConnectionDetails { get; set; }
    }

    public class PowerBIActivityLogSubscribeeInformation {
      public string RecipientEmail { get; set; }
      public string RecipientName { get; set; }
      public string ObjectId { get; set; }
    }

    public class PowerBIActivityLogExportedArtifactInfo {
      public string ExportType { get; set; }
      public string ArtifactType { get; set; }
      public int ArtifactId { get; set; }
    }

    public class PowerBIActivityLogAuditedArtifactInformation {
      public string Id { get; set; }
      public string Name { get; set; }
      public string ArtifactObjectId { get; set; }
      public string AnnotatedItemType { get; set; }
    }

  }

}
