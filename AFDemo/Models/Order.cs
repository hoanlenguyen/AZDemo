using System;
using System.ComponentModel.DataAnnotations;

namespace AFDemo.Models
{
    public class Order
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string OrderNumber { get; set; }

        [MaxLength(20)]
        public string ProductSKu { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        [MaxLength(20)]
        public string OrderStatus { get; set; }
    }
}