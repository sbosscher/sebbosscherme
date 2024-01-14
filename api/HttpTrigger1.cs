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
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public static class IncrementValueFunction
{
    [FunctionName("IncrementValueFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string keyVaultUrl = Environment.GetEnvironmentVariable("KeyVaultUrl", EnvironmentVariableTarget.Process);
        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        KeyVaultSecret connectionStringSecret = await client.GetSecretAsync("connectionString");
        KeyVaultSecret tableNameSecret = await client.GetSecretAsync("tableName");
        KeyVaultSecret partitionKeySecret = await client.GetSecretAsync("partitionKey");
        KeyVaultSecret rowKeySecret = await client.GetSecretAsync("rowKey");

        string connectionString = connectionStringSecret.Value;
        string tableName = tableNameSecret.Value;
        string partitionKey = partitionKeySecret.Value;
        string rowKey = rowKeySecret.Value;

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