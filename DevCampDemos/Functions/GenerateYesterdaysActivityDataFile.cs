using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;

using DevCampDemos.Services;

namespace DevCampDemos.Functions {

  public static class GenerateYesterdaysActivityDataFile {

    [FunctionName("GenerateYesterdaysActivityDataFile")]
    public static async Task Run(
        [TimerTrigger("0 30 6 * * *", RunOnStartup = false)] TimerInfo myTimer) {

      var yesterday = DateTime.Now.AddDays(-1);
      string fileName = "pbievents-" + yesterday.ToString("yyyy-MM-dd") + ".json";

      var events = await PowerBiManager.GetActivityEvents(yesterday);
      var json = PowerBiManager.ConvertObjectToJson(events);
      
      string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
      string containerName = Environment.GetEnvironmentVariable("EventsContainerName");     

      var blobClient = new BlobContainerClient(Connection, containerName);
      var blob = blobClient.GetBlobClient(fileName);
      await blob.UploadAsync(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)), true);

    }
  }
}