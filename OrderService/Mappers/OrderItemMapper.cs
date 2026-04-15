using OrderService.DTO.OrderItem;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemDTO ToOrderItemDTO(Entities.OrderItem orderItem)
        {
            return new OrderItemDTO
            {
                IdOrderItem = orderItem.IdOrderItem,
                Quantity = orderItem.Quantity,
                PricePerItem = orderItem.PricePerItem,
                MenuItemDTO = new DTO.MenuItem.MenuItemDTO
                {
                    IdMenuItem = orderItem.IdMenuItem,
                }
            };
        }

        public static Entities.OrderItem ToOrderItem(CreateOrderItemDTO createOrderItemDTO)
        {
            return new Entities.OrderItem
            {
                IdMenuItem = createOrderItemDTO.IdMenuItem,
                Quantity = createOrderItemDTO.Quantity,
                PricePerItem = createOrderItemDTO.PricePerItem,
            };
        }
    }
}
