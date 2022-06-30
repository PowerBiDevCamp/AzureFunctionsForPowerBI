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
using Azure.Storage.Blobs;

namespace DevCampDemos.Functions {
  public static class GenerateActivityDataFileForDate {

    [FunctionName("GenerateActivityDataFileForDate")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req) {

      string dateString = req.Query["date"];
      DateTime date;
      bool DateIsValid = DateTime.TryParse(dateString, out date);

      if (DateIsValid) {
        string fileName = "pbievents-" + date.ToString("yyyy-MM-dd") + ".json";

        var events = await PowerBiManager.GetActivityEvents(date);
        var json = PowerBiManager.ConvertObjectToJson(events);

        string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        string containerName = Environment.GetEnvironmentVariable("EventsContainerName");

        var blobClient = new BlobContainerClient(Connection, containerName);
        var blob = blobClient.GetBlobClient(fileName);
        await blob.UploadAsync(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)));

        return new OkObjectResult("Activity data file generated - " + fileName);
      }
      else {
        return new BadRequestObjectResult("Date is invalid");
      }

      
    }
  }
}
