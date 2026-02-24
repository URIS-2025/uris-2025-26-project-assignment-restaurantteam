using OrderService.Entities.Enums;
using System;
using System.Collections.Generic;

namespace OrderService.Entities
{
  
    public class Order
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}