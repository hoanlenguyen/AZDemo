using AFDemo.Data;
using AFDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly MyDbContext db;

        public OrderService(MyDbContext db)
        {
            this.db = db;
        }

        public Order InitializeOrder(Order order)
        {
            order.OrderNumber = Guid.NewGuid().ToString();
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
            order.OrderStatus = nameof(SaveOrder);
            db.Add(order);
            db.SaveChanges();
            return order;
        }

        public List<Order> GetOrders(DateTime dateTime)
        {
            return db.Orders.AsNoTracking().OrderByDescending(p => p.Id).Take(5).ToList();
            //return new List<Order>()
            //{
            //    new Order(){OrderNumber = "ON-00001",Id=1 , Quantity=1},
            //    new Order(){OrderNumber = "ON-00002",Id=2 , Quantity=2},
            //    new Order(){OrderNumber = "ON-00003",Id=3 , Quantity=3},
            //    new Order(){OrderNumber = "ON-00004",Id=4 , Quantity=4},
            //    new Order(){OrderNumber = "ON-00005",Id=5 , Quantity=5}
            //};
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