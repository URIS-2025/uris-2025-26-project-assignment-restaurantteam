using OrderService.Entities;

namespace OrderService.Payloads
{
    public class OrderResponse
    {
        public int IdOrder { get; set; }
        public int IdUser { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
    }
}
