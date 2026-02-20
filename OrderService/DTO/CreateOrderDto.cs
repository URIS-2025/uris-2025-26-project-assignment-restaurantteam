using System;
using System.Collections.Generic;

namespace OrderService.DTO
{
    public class CreateOrderDto
    {
        public int IdUser { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}