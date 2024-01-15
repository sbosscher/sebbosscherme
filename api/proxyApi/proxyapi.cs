using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public static class ProxyApi
{
    private static readonly HttpClient client = new HttpClient();

    [FunctionName("ProxyApi")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var functionUrl = Environment.GetEnvironmentVariable("EXTERNAL_FUNCTION_URL");
        var functionKey = Environment.GetEnvironmentVariable("EXTERNAL_FUNCTION_KEY");

        var response = await client.GetStringAsync($"{functionUrl}?code={functionKey}");

        return new OkObjectResult(response);
    }
}