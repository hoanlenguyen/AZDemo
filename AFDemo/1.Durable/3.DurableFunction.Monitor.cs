using AFDemo.Models;
using AFDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AFDemo
{
    public class DurableFunctionMonitor
    {
        [FunctionName(nameof(DurableFunctionMonitor))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            string jobId = context.GetInput<string>();
            int pollingInterval = GetPollingInterval();
            DateTime expiryTime = GetExpiryTime(context);
            //add a job process for test 
            
            //await context.CallActivityAsync(nameof(OrderActivity.SendAlert), Constants.Completed);

            //
            while (context.CurrentUtcDateTime < expiryTime)
            {
                var jobStatus = await context.CallActivityAsync<string>(nameof(OrderActivity.GetJobStatus), jobId);
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
        }

        [FunctionName($"{nameof(DurableFunctionMonitor)}_HttpStart")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var jobId = req.Query["jobId"];
            var instanceId = await starter.StartNewAsync(nameof(DurableFunctionMonitor), jobId);
            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private DateTime GetExpiryTime(IDurableOrchestrationContext context)
        {
            return context.CurrentUtcDateTime.AddMinutes(5); //Define the expiry time
        }

        private int GetPollingInterval()
        {
            return 15; //Define the polling interval
        }
    }
}