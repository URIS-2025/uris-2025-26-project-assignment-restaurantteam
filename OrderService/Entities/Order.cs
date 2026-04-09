using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        [Required]
        public int IdUser { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        [Range(0.01, 100000)]
        public decimal TotalPrice { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        PENDING,
        PREPARING,
        READY,
        COMPLETED,
        CANCELED
    }

    public enum PaymentMethod
    {
        CASH,
        CARD
    }
}
