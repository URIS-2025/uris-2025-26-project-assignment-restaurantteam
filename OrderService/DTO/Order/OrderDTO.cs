using OrderService.Entities;
using System.ComponentModel.DataAnnotations;
using OrderService.DTO.User;
using OrderService.DTO.OrderItem;

namespace OrderService.DTO.Order
{
    public class OrderDTO
    {
        [Key]
        public int IdOrder { get; set; }

        public UserDTO UserDTO { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Range(0.01, 100000)]
        public decimal TotalPrice { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
