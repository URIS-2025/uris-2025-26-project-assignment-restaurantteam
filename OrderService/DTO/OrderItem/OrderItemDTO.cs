using System.ComponentModel.DataAnnotations;
using OrderService.DTO.MenuItem;

namespace OrderService.DTO.OrderItem
{
    public class OrderItemDTO
    {
        [Key]
        public int IdOrderItem { get; set; }
        public MenuItemDTO MenuItemDTO { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal PricePerItem { get; set; }
    }
}
