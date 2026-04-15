using OrderService.DTO.MenuItem;
using System.ComponentModel.DataAnnotations;

namespace OrderService.DTO.OrderItem
{
    public class CreateOrderItemDTO
    {
        [Required]
        public int IdMenuItem { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Range(0, 10000)]
        public decimal PricePerItem { get; set; } // should take the price of the menu item
    }
}
