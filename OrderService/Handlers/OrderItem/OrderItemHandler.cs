using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO.Order;
using OrderService.DTO.OrderItem;
using OrderService.Entities;
using OrderService.Mappers;

namespace OrderService.Handlers.OrderItem
{
    public class OrderItemHandler : IOrderItemHandler
    {
        private readonly OrderDbContext _context;
        public OrderItemHandler(OrderDbContext context) 
        { 
            _context = context;
        }

        public async Task<OrderItemDTO> CreateOrderItem(CreateOrderItemDTO createOrderItemDTO, int idOrder)
        {
            var orderItem = OrderItemMapper.ToOrderItem(createOrderItemDTO);
            orderItem.IdOrder = idOrder;
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            var orderItemDTO = OrderItemMapper.ToOrderItemDTO(orderItem);

            return orderItemDTO;
        }

        public async Task<bool> DeleteOrderItem(int idOrderItem)
        {
            Entities.OrderItem? orderItem = await _context.OrderItems.FindAsync(idOrderItem);

            if (orderItem == null)
            {
                throw new Exception("Order item not found");
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderItemDTO> GetOrderItemById(int idOrderItem)
        {
            Entities.OrderItem? orderItem = await _context.OrderItems.FindAsync(idOrderItem);


            if (orderItem == null)
            {
                throw new Exception("Order item not found");

            }

            return OrderItemMapper.ToOrderItemDTO(orderItem);
        }

        public Task<List<OrderItemDTO>> GetOrderItems()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItemDTO> UpdateOrderItem(UpdateOrderItemDTO updateOrderItemDTO, int idOrderItem)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsMenuItemInUse(int idMenuItem)
        {
            var orderItems = _context.OrderItems.Where(oi => oi.IdMenuItem == idMenuItem).ToList();
            if (orderItems == null)
                return false;
            return true;
        }
    }
}
