using AFDemo.Models;
using AFDemo.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace AFDemo
{
    public class DurableFunctionChaining
    {
        [FunctionName(nameof(DurableFunctionChaining))]
        public async Task<object> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var order = context.GetInput<Order>();

            order = await context.CallActivityAsync<Order>(nameof(IOrderService.InitializeOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.ValidateOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.ProcessOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.SaveOrder), order);
            return order;
        }

        [FunctionName($"{nameof(DurableFunctionChaining)}_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
           [DurableClient] IDurableOrchestrationClient starter,
           ILogger log)
        {
            var instanceId = await starter.StartNewAsync(nameof(DurableFunctionChaining), await req.Content.ReadAsAsync<Order>());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}