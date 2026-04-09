using OrderService.Entities;

namespace OrderService.Payloads
{
    public class UpdateOrderStatus
    {
        public OrderStatus Status { get; set; }
    }
}
