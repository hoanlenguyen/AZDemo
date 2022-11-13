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

        string GetJobStatus(string jobId);

        void CreateProcessingJob(string jobId);

        int CompletedProcessingJob(string jobId);
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
        }

        public void SendNotification(IEnumerable<string> orderNumbers)
        {
            //TODO: Send notification
            foreach (var orderNumber in orderNumbers)
            {
                //Send orderNumber
            }
        }

        public string GetJobStatus(string jobId)
        {
            var job= db.ProcessOrderStatuses.FirstOrDefault(p => p.JobId == jobId);
            var status = job != null ? job.Status : Constants.Running;
            return status;
        }

        public void CreateProcessingJob(string jobId)
        {
            db.ProcessOrderStatuses.Add(new ProcessOrderStatus
            {
                JobId = jobId,
                Status = Constants.Running
            });
            db.SaveChanges();
        }

        public int CompletedProcessingJob(string jobId)
        {
            var job = db.ProcessOrderStatuses.FirstOrDefault(p => p.JobId == jobId);
            if (job != null)
            {
                job.Status = Constants.Completed;
                db.SaveChanges();
                return job.Id;
            }
            return 0;
        }
    }
}