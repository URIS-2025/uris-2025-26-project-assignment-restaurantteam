using OrderService.DTO.Order;

namespace OrderService.Handlers.Order
{
    public interface IOrderHandler
    {
        public Task<OrderDTO> CreateOrder(CreateOrderDTO createOrderDTO, int idUser);
        public Task<List<OrderDTO>> GetOrders();
        public Task<OrderDTO> GetOrderById(int idOrder);
        public Task<OrderDTO> UpdateOrder(UpdateOrderDTO updateOrderDTO, int idUser);
        public Task<bool> DeleteOrder(int idOrder);
    }
}
