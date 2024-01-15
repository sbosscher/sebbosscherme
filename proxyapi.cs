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

        var response = await client.GetStringAsync("https://sbosscher-f.azurewebsites.net/api/incrementvaluefunction?code=f5gGUYkK_1kM9baSpu-m5T0FEpxHw2ELZUKjg3EABDg4AzFutBA18Q%3D%3D");

        return new OkObjectResult(response);
    }
}