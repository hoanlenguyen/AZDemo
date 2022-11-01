using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFDemo
{
    public interface IOrderService
    {
        Order InitializeOrder(Order order);
        Order ValidateOrder(Order order);
        Order ProcessOrder(Order order);
        Order SaveOrder(Order order);
    }

    public class OrderService : IOrderService
    {
        public Order InitializeOrder(Order order)
        {
            order.OrderNumber = $"ON-RANDOM"; // generate number
            order.OrderStatus = nameof(InitializeOrder);
            return order;
        }

        public Order ValidateOrder(Order order)
        {
            if (order.Quantity == 0) // validate quantity
            {
                order.Quantity = 1; // at least one item
            }

            order.OrderStatus = nameof(ValidateOrder);
            return order;
        }

        public Order ProcessOrder(Order order)
        {
            order.OrderDate = DateTime.Now; // generate date
            order.OrderStatus = nameof(ProcessOrder);
            return order;
        }

        public Order SaveOrder(Order order)
        {
            order.OrderId = 1000; // generate id
            order.OrderStatus = nameof(SaveOrder);
            return order;
        }
    }
}
