using System.IO;
using System.Threading.Tasks;
using DevCampDemos.Services;
using Microsoft.Azure.WebJobs;

namespace DevCampDemos.Functions {

  public class GenerateWorkspacesDataFile {

    [FunctionName("GenerateWorkspacesDataFile")]
    public async Task Run([TimerTrigger("0 30 6 * * *", RunOnStartup = false)] TimerInfo myTimer,
                          [Blob("pbidata/Workspaces.json", FileAccess.Write)] TextWriter streamWriter) {

      // call to Power BI REST API to get workspace inventory
      var workspaces = await PowerBiManager.GetWorkspacesAsAdminWithExpand();

      // Save response data in file in Azure storage
      streamWriter.Write(PowerBiManager.ConvertObjectToJson(workspaces));

    }
  }
}

