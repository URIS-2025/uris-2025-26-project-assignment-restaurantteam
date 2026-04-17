using OrderService.DTO.Order;

namespace OrderService.Handlers.Order
{
    public interface IOrderHandler
    {
        public Task<OrderDTO> CreateOrder(CreateOrderDTO createOrderDTO);
        public Task<List<OrderDTO>> GetOrders();
        public Task<OrderDTO> GetOrderById(int idOrder);
        public Task<OrderDTO> UpdateOrder(UpdateOrderDTO updateOrderDTO, int idUser);
        public Task<OrderDTO> UpdateOrderStatus(UpdateOrderDTO updateOrderDTO, int idUser);
        public Task<bool> IsUserInUse(int idUser);
        public Task<bool> DeleteOrder(int idOrder);
    }
}
