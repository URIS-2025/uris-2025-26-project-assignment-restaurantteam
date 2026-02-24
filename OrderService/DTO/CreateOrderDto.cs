using System.Collections.Generic;
using OrderService.Entities.Enums;

namespace OrderService.DTO
{
    public class CreateOrderDto
    {
        public PaymentMethod PaymentMethod { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }

    public class CreateOrderItemDto
    {
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}