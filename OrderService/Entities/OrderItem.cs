using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities
{
    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }
        public int IdMenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
    }
}