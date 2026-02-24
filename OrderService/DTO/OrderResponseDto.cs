using System;
using System.Collections.Generic;
using OrderService.Entities.Enums;

namespace OrderService.DTO
{
    public class OrderResponseDto
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}