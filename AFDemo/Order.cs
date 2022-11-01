using Newtonsoft.Json;
using System;

namespace AFDemo
{
    public class Order
    {
        [JsonProperty("OrderId")]
        public int OrderId { get; set; }

        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("ProductSKu")]
        public string ProductSKu { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("OrderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("OrderStatus")]
        public string OrderStatus { get; set; }
    }
}