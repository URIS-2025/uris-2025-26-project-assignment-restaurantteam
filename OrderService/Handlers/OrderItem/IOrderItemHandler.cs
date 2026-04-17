using OrderService.DTO.OrderItem;

namespace OrderService.Handlers.OrderItem
{
    public interface IOrderItemHandler
    {
        public Task<OrderItemDTO> CreateOrderItem(CreateOrderItemDTO createOrderItemDTO, int idOrder);
        public Task<List<OrderItemDTO>> GetOrderItems();
        public Task<OrderItemDTO> GetOrderItemById(int idOrderItem);
        public Task<OrderItemDTO> UpdateOrderItem(UpdateOrderItemDTO updateOrderItemDTO, int idOrderItem);
        public Task<bool> DeleteOrderItem(int idOrderItem);
        public Task<bool> IsMenuItemInUse(int idMenuItem);
    }
}
