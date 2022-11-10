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

        [FunctionName(nameof(IOrderService.InitializeOrder))]
        public Order Initialize([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Initializing order.");
            var initializedOrder = _orderService.InitializeOrder(order);
            return initializedOrder;
        }

        [FunctionName(nameof(IOrderService.ValidateOrder))]
        public Order ValidateOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Validating order.");
            var validatedOrder = _orderService.ValidateOrder(order);
            return validatedOrder;
        }

        [FunctionName(nameof(IOrderService.ProcessOrder))]
        public async Task<Order> ProcessOrderAsync([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Processing order.");
            var processedOrder = _orderService.ProcessOrder(order);
            //await Task.Delay(20000);

            return processedOrder;
        }

        [FunctionName(nameof(IOrderService.SaveOrder))]
        public Order SaveOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Saving order.");
            var savedOrder = _orderService.SaveOrder(order);
            return savedOrder;
        }

        [FunctionName(nameof(IOrderService.GetOrders))]
        public List<Order> GetOrders([ActivityTrigger] DateTime dateTime, ILogger log)
        {
            log.LogInformation($"Getting orders.");
            var orders = _orderService.GetOrders(dateTime);
            return orders;
        }

        [FunctionName(nameof(IOrderService.SendNotification))]
        public void SendNotification([ActivityTrigger] string[] orderNumbers, ILogger log)
        {
            log.LogInformation($"Send notification.");
            _orderService.SendNotification(orderNumbers);
        }

        [FunctionName(nameof(GetJobStatus))]
        public string GetJobStatus([ActivityTrigger] string jobId, ILogger log)
        {
            //log.LogWarning($"SendAlert :{jobId}");
            return Constants.Running;
        }

        [FunctionName(nameof(SendAlert))]
        public void SendAlert([ActivityTrigger] string status, ILogger log)
        {
            log.LogWarning($"SendAlert :{status}");
        }
    }
}