using AFDemo.Models;
using System;
using System.Collections.Generic;

namespace AFDemo.Services
{
    public interface IOrderService
    {
        Order InitializeOrder(Order order);

        Order ValidateOrder(Order order);

        Order ProcessOrder(Order order);

        Order SaveOrder(Order order);

        List<Order> GetOrders(DateTime dateTime);

        void SendNotification(IEnumerable<string> orderNumbers);
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

        public List<Order> GetOrders(DateTime dateTime)
        {
            //TODO: Retrieve orders for a particular date

            return new List<Order>()
            {
                new Order(){OrderNumber = "ON-00001",OrderId=1 , Quantity=1},
                new Order(){OrderNumber = "ON-00002",OrderId=2 , Quantity=2},
                new Order(){OrderNumber = "ON-00003",OrderId=3 , Quantity=3},
                new Order(){OrderNumber = "ON-00004",OrderId=4 , Quantity=4},
                new Order(){OrderNumber = "ON-00005",OrderId=5 , Quantity=5}
            };
        }

        public void SendNotification(IEnumerable<string> orderNumbers)
        {
            //TODO: Send notification
            foreach (var orderNumber in orderNumbers)
            {
                //Send orderNumber
            }
        }
    }
}