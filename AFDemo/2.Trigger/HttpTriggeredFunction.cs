using AFDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AFDemo
{
    public class HttpTriggeredFunction
    {
        private readonly IOrderService orderService;

        public HttpTriggeredFunction(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [FunctionName(nameof(GetOrdersByFilter))]
        public async Task<IActionResult> GetOrdersByFilter(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            //string name = req.Query["name"];          

            return new OkObjectResult(orderService.GetOrders(DateTime.Now));
        }


        [FunctionName($"{nameof(GetJobStatus)}_Http_Start")]
        public async Task<IActionResult> GetJobStatus(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string jobId = req.Query["jobId"];
            return new OkObjectResult(orderService.GetJobStatus(jobId));
        }

        [FunctionName(nameof(CompletedProcessingJob))]
        public async Task<IActionResult> CompletedProcessingJob(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string jobId = req.Query["jobId"];  
            var id = orderService.CompletedProcessingJob(jobId);
            if(id == 0)
                log.LogError($"Can not find {jobId}");

            return new OkObjectResult(jobId);
        }
    }
}