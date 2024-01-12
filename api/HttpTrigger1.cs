using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure; // Add this line

public static class IncrementValueFunction
{
    [FunctionName("IncrementValueFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=sebbosschermegroup8c6a;AccountKey=klhn8XKDrqWT/gdUDEeZFfa3jP+IV2El1hozngh2eVXkjAN32u8giIfBguURkkaCaXC3Ycrm8z2L+AStnVbULQ==;EndpointSuffix=core.windows.net";
        string tableName = "visits";
        string partitionKey = "WebsiteData";
        string rowKey = "VisitorCount"; // Add this line

        // Create a new TableClient
        var tableClient = new TableClient(connectionString, tableName);

        // Get the entity
        var response = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey); // Update this line
        var entity = response.Value;

        // Get the current value and increment it
        int currentValue = (int)entity["Count"];
        currentValue++;

        // Update the entity with the new value
        entity["Count"] = currentValue;
        await tableClient.UpdateEntityAsync(entity, ETag.All);

        // Return the new value
        return new OkObjectResult(currentValue);
    }
}