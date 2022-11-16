using AFDemo.Models;
using AFDemo.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AFDemo
{
    public class OrderActivity
    {
        private readonly IOrderService _orderService;

        public OrderActivity(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [FunctionName(nameof(InitializeOrder))]
        public Order InitializeOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Initializing order.");
            var initializedOrder = _orderService.InitializeOrder(order);
            return initializedOrder;
        }

        [FunctionName(nameof(ValidateOrder))]
        public Order ValidateOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Validating order.");
            var validatedOrder = _orderService.ValidateOrder(order);
            return validatedOrder;
        }

        [FunctionName(nameof(ProcessOrder))]
        public async Task<Order> ProcessOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Processing order.");
            var processedOrder = _orderService.ProcessOrder(order);
            //await Task.Delay(60000);
            return processedOrder;
        }

        [FunctionName(nameof(SaveOrder))]
        public Order SaveOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Saving order.");
            var savedOrder = _orderService.SaveOrder(order);
            return savedOrder;
        }

        [FunctionName(nameof(GetOrders))]
        public List<Order> GetOrders([ActivityTrigger] DateTime dateTime, ILogger log)
        {
            log.LogInformation($"Getting orders.");
            var orders = _orderService.GetOrders(dateTime);
            return orders;
        }

        [FunctionName(nameof(SendNotification))]
        public void SendNotification([ActivityTrigger] string[] orderNumbers, ILogger log)
        {
            log.LogInformation($"Send notification.");
            _orderService.SendNotification(orderNumbers);
        }

        [FunctionName(nameof(GetJobStatus))]
        public string GetJobStatus([ActivityTrigger] string jobId, ILogger log)
        {
            var status = _orderService.GetJobStatus(jobId);
            return status;
        }
 
        [FunctionName(nameof(SendAlert))]
        public void SendAlert([ActivityTrigger] string status, ILogger log)
        {
            log.LogWarning($"SendAlert :{status}");
        }
    }
}