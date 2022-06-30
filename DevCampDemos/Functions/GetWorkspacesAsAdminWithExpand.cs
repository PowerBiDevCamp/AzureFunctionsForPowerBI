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

  public static class GetWorkspacesAsAdminWithExpand {
  
    [FunctionName("GetWorkspacesAsAdminWithExpand")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {

      var workspaces = await PowerBiManager.GetWorkspacesAsAdminWithExpand();
      return new JsonResult(workspaces);

    }
  }
}
