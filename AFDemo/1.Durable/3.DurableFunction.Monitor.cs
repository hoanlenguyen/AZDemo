using AFDemo.Models;
using AFDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AFDemo
{
    public class DurableFunctionMonitor
    {
        private readonly IOrderService _orderService;
        public DurableFunctionMonitor(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [FunctionName(nameof(DurableFunctionMonitor))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var job = context.GetInput<ProcessOrderStatus>();
            log.LogWarning($"DurableFunctionMonitor - jobId: {job.JobId}");
            int pollingInterval = GetPollingInterval();
            DateTime expiryTime = GetExpiryTime(context);
             
            while (context.CurrentUtcDateTime < expiryTime)
            {
                var jobStatus = await context.CallActivityAsync<string>(nameof(OrderActivity.GetJobStatus), job.JobId);
                if (jobStatus == Constants.Completed)
                {
                    // Perform an action when a condition is met.
                    await context.CallActivityAsync(nameof(OrderActivity.SendAlert), Constants.Completed);
                    break;
                }
                else
                {
                    await context.CallActivityAsync(nameof(OrderActivity.SendAlert), Constants.Running);
                }

                // Orchestration sleeps until this time.
                var nextCheck = context.CurrentUtcDateTime.AddSeconds(pollingInterval);
                await context.CreateTimer(nextCheck, CancellationToken.None);
            }

            // Perform more work here, or let the orchestration end.
            await context.CallActivityAsync(nameof(OrderActivity.SendAlert), Constants.EndMonitor);
        }
         
        [FunctionName($"{nameof(DurableFunctionMonitor)}_HttpStart")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var jobId = req.Query["jobId"];
            log.LogWarning($"jobId: {jobId}");

            //Create a mock Running Job for test DurableFunctionMonitor
            CreateRunningTestJob(jobId);

            var instanceId = await starter.StartNewAsync(nameof(DurableFunctionMonitor), new ProcessOrderStatus { JobId = jobId });
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
        
        private void CreateRunningTestJob(string jobId)
        {
            _orderService.CreateProcessingJob(jobId);
        }

        private DateTime GetExpiryTime(IDurableOrchestrationContext context)
        {
            return context.CurrentUtcDateTime.AddMinutes(5); //Define the expiry time
        }

        private int GetPollingInterval()
        {
            return 3; //Define the polling interval
        }
    }
}