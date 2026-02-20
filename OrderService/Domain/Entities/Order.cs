using System;
using System.Collections.Generic;
using OrderService.Domain.Enums;
using OrderService.Domain.ValueObjects;

namespace OrderService.Domain.Entities
{
    public class Order
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Money TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Stavke porudžbine
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}