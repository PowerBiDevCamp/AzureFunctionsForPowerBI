using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DevCampDemos.Services;

namespace DevCampDemos.Functions {
  public static class ScanWorkspace {

    [FunctionName("ScanWorkspace")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req) {

      string workspace = req.Query["workspace"];
      Guid workspaceId;
      bool isGuid = Guid.TryParse(workspace, out workspaceId);

      if (isGuid) {
        var scanResult = await PowerBiManager.ScanWorkspaceAsAdmin(workspaceId);
        return new JsonResult(scanResult);
      }
      else {
        return new BadRequestObjectResult("Workspace ID not valid");
      }

    }
  }
}
