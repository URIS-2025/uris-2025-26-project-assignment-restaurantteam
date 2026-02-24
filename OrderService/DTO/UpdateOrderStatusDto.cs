using OrderService.Entities.Enums;

namespace OrderService.DTO
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}