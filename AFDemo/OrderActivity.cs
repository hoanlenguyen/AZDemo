using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

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
        public Order ProcessOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Processing order.");
            var processedOrder = _orderService.ProcessOrder(order);
            return processedOrder;
        }

        [FunctionName(nameof(IOrderService.SaveOrder))]
        public Order SaveOrder([ActivityTrigger] Order order, ILogger log)
        {
            log.LogInformation($"Saving order.");
            var savedOrder = _orderService.SaveOrder(order);
            return savedOrder;
        }
    }
}