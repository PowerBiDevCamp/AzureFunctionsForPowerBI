using System;
using System.IO;
using System.Threading.Tasks;
using DevCampDemos.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace DevCampDemos.Functions {

  public class ProcessDataFileUpload {

    [FunctionName("ProcessDataFileUpload")]
    public async Task Run([BlobTrigger("salesdata/{fileName}")] Stream salesDataStream, string fileName,
                          [Blob("templates/SalesReportTemplate.pbix", FileAccess.Read)] Stream pbixTemplateStream,
                          ILogger log) {

      Guid WorkspaceId = new Guid(Environment.GetEnvironmentVariable("SalesReportsWorkspaceId"));

      await PowerBiManager.ProcessFileUpload(WorkspaceId, pbixTemplateStream, fileName);

      log.LogInformation($"Processed CSV File: {fileName}");

    }
  }
}
