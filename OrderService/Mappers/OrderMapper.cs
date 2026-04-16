using OrderService.DTO.Order;
using OrderService.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Mappers
{
    public static class OrderMapper
    {
        public static Entities.Order ToOrder(CreateOrderDTO createOrderDTO)
        {
            return new Entities.Order
            {
                IdUser = createOrderDTO.IdUser ?? 1,
                OrderStatus = createOrderDTO.OrderStatus ?? Entities.OrderStatus.PENDING,
                TotalPrice = createOrderDTO.TotalPrice ?? 0,
                PaymentMethod = createOrderDTO.PaymentMethod,
                CreatedAt = createOrderDTO.CreatedAt ?? DateTime.Now,
            };
        }

        public static OrderDTO ToOrderDTO(Entities.Order order)
        {
            return new OrderDTO
            {
                IdOrder = order.IdOrder,
                OrderStatus = order.OrderStatus,
                TotalPrice = order.TotalPrice,
                PaymentMethod = order.PaymentMethod,
                CreatedAt = order.CreatedAt,
                UserDTO = new DTO.User.UserDTO
                {
                    IdUser = order.IdUser,
                },
                
            };
        }
    }
}
