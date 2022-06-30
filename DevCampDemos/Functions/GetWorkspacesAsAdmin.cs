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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevCampDemos.Functions {

  public static class GetWorkspacesAsAdmin {

    [FunctionName("GetWorkspacesAsAdmin")]
    public static async Task<JsonResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {

      // call to Power BI REST API
      var workspaces = await PowerBiManager.GetWorkspacesAsAdmin();

      // return response data to caller
      return new JsonResult(workspaces);

    }
  }
}