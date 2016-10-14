using System;
using System.Collections.Generic;

namespace OrderProcess
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int Quantity { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
    }

    public class Order
    {
        public Order()
        {
            Items = new List<OrderItem>();
        }

        public int OrderID { get; set; }
        public string Description { get; set; }
        public decimal TotalWeight { get; set; }
        public string ShippingMethod { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
