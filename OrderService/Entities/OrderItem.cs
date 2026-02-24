using System;

namespace OrderService.Entities
{
    public class OrderItem
    {
        public int IdOrderItem { get; set; }
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}