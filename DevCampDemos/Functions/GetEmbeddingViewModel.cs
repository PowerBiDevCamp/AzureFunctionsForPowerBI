using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

using DevCampDemos.Services;

namespace DevCampDemos.Functions {

  public static class GetEmbeddingViewModel {

    [FunctionName("GetEmbeddingViewModel")]
    public static async Task<JsonResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req) {

      var viewmodel = await PowerBiManager.GetEmbeddedViewModel();
      return new JsonResult(viewmodel);

    }
  }
}