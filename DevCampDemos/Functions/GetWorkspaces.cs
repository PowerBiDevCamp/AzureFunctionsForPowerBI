using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

using DevCampDemos.Services;

namespace DevCampDemos.Functions {

  public static class GetWorkspaces {

    [FunctionName("GetWorkspaces")]
    public static async Task<JsonResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {

      // call to Power BI REST API
      var workspaces = await PowerBiManager.GetWorkspaces();

      // return response data to caller
      return new JsonResult(workspaces);

    }
  }
}


