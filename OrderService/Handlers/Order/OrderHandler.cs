using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO.Order;
using OrderService.DTO.OrderItem;

using OrderService.Mappers;
using System.Reflection.Metadata.Ecma335;

namespace OrderService.Handlers.Order
{
    public class OrderHandler : IOrderHandler
    {
        private readonly OrderDbContext _context;

        public OrderHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDTO> CreateOrder(CreateOrderDTO createOrderDTO)
        {
            var order = OrderMapper.ToOrder(createOrderDTO);

            _context.Orders.Add(order);
            

            await _context.SaveChangesAsync();

            var orderDTO = OrderMapper.ToOrderDTO(order);
            
            return orderDTO;
        }

        public async Task<bool> DeleteOrder(int idOrder)
        {
            Entities.Order? order = await _context.Orders.FindAsync(idOrder);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrderDTO> GetOrderById(int idOrder)
        {
            Entities.Order? order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.IdOrder == idOrder);

            if (order == null)
            {
                throw new Exception("Order not found");

            }
            var orderDTO = OrderMapper.ToOrderDTO(order);
            var orderItemDTOs = new List<OrderItemDTO>();
            foreach (Entities.OrderItem orderItem in order.OrderItems)
            {
                orderItemDTOs.Add(OrderItemMapper.ToOrderItemDTO(orderItem));
            }
            orderDTO.OrderItems = orderItemDTOs;

            return orderDTO;
        }

        public async Task<List<OrderDTO>> GetOrders()
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ToList();

            var orderDTOs = new List<OrderDTO>();
            foreach (Entities.Order order in orders)
            {
                var orderItemDTOs = new List<OrderItemDTO>();
    

                foreach (Entities.OrderItem orderItem in order.OrderItems)
                {
                    orderItemDTOs.Add(OrderItemMapper.ToOrderItemDTO(orderItem));
                }
                var orderDTO = OrderMapper.ToOrderDTO(order);
                orderDTO.OrderItems = orderItemDTOs;
                
                orderDTOs.Add(orderDTO);

            }
            return orderDTOs;
        }

        public async Task<OrderDTO> UpdateOrder(UpdateOrderDTO updateOrderDTO, int idOrder)
        {
            Entities.Order? order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.IdOrder == idOrder);

            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if(updateOrderDTO.PaymentMethod !=null)
                order.PaymentMethod = updateOrderDTO.PaymentMethod;

            if (updateOrderDTO.TotalPrice != null)
                order.TotalPrice = updateOrderDTO.TotalPrice;

            if (updateOrderDTO.OrderStatus != null)
                order.OrderStatus = updateOrderDTO.OrderStatus;

               


            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            OrderDTO orderDTO = OrderMapper.ToOrderDTO(order);
            var orderItemDTOs = new List<OrderItemDTO>();
            foreach (Entities.OrderItem orderItem in order.OrderItems)
            {
                orderItemDTOs.Add(OrderItemMapper.ToOrderItemDTO(orderItem));
            }
            orderDTO.OrderItems = orderItemDTOs;

            return orderDTO;
        }

        public async Task<OrderDTO> UpdateOrderStatus(UpdateOrderDTO updateOrderDTO, int idOrder)
        {
            Entities.Order? order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.IdOrder == idOrder);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (updateOrderDTO.OrderStatus != null)
                order.OrderStatus = updateOrderDTO.OrderStatus;




            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            OrderDTO orderDTO = OrderMapper.ToOrderDTO(order);
            var orderItemDTOs = new List<OrderItemDTO>();
            foreach (Entities.OrderItem orderItem in order.OrderItems)
            {
                orderItemDTOs.Add(OrderItemMapper.ToOrderItemDTO(orderItem));
            }
            orderDTO.OrderItems = orderItemDTOs;

            return orderDTO;
        }

        public async Task<bool> IsUserInUse(int idUser)
        {
            bool orders = await _context.Orders.AnyAsync(o => o.IdUser == idUser);
            return orders;
        }
    }
}
