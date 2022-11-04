using AFDemo.Models;
using AFDemo.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AFDemo
{
    public class DurableFunctionFanOutIn
    {
        [FunctionName(nameof(DurableFunctionFanOutIn))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var orderProcessingTasks = new List<Task<Order>>();

            // Get a list of orders
            var orders = await context.CallActivityAsync<List<Order>>(nameof(IOrderService.GetOrders), DateTime.Now);

            foreach (var order in orders)
            {
                // Process orders in parallel
                Task<Order> task = context.CallActivityAsync<Order>(nameof(IOrderService.ProcessOrder), order);
                orderProcessingTasks.Add(task);
            }

            // Wait for all orders to process
            var orderNumbers = await Task.WhenAll(orderProcessingTasks);

            // Send notification
            await context.CallActivityAsync(nameof(IOrderService.SendNotification), orderNumbers.Select(p => p.OrderNumber));
        }

        [FunctionName($"{nameof(DurableFunctionFanOutIn)}_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var instanceId = await starter.StartNewAsync(nameof(DurableFunctionFanOutIn));

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}