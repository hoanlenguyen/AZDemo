using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AFDemo
{
    public class DurableFunctionChainingDemo
    {
        [FunctionName(nameof(RunOrchestrator))]
        public async Task<object> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<JObject>();
            var order = JsonConvert.DeserializeObject<Order>(input.ToString());

            order = await context.CallActivityAsync<Order>(nameof(IOrderService.InitializeOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.ValidateOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.ProcessOrder), order);
            order = await context.CallActivityAsync<Order>(nameof(IOrderService.SaveOrder), order);
            return order;
        }

        [FunctionName("DurableFunctionChaining_HttpStart")]
        public async Task<HttpResponseMessage> HttpStart(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
           [DurableClient] IDurableOrchestrationClient starter,
           ILogger log)
        {

            var instanceId = await starter.StartNewAsync(nameof(RunOrchestrator), await req.Content.ReadAsAsync<Order>());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}