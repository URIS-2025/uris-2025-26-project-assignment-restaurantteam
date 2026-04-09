using OrderService.Entities;

namespace OrderService.Payloads
{
    public class CreateOrder
    {
        public PaymentMethod PaymentMethod { get; set; }
        public List<CreateOrderItem> Items { get; set; } = new List<CreateOrderItem>();
    }
}
