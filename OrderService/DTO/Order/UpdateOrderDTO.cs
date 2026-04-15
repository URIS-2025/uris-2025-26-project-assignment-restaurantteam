using OrderService.DTO.OrderItem;
using OrderService.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderService.DTO.Order
{
    public class UpdateOrderDTO
    {
 
        public OrderStatus OrderStatus { get; set; }

        [Range(0, 100000)]
        public decimal TotalPrice { get; set; }

        public PaymentMethod PaymentMethod { get; set; }


        public DateTime CreatedAt { get; set; }

        public List<CreateOrderItemDTO> OrderItems { get; set; } = new List<CreateOrderItemDTO>();
    }
}
