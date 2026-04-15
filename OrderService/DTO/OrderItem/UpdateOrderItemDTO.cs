using System.ComponentModel.DataAnnotations;

namespace OrderService.DTO.OrderItem
{
    public class UpdateOrderItemDTO
    {
        [Key]
        public int IdOrderItem { get; set; }

        [Required]
        public int IdMenuItem { get; set; }

        [Required]
        public int IdOrder { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal PricePerItem { get; set; }
    }
}
