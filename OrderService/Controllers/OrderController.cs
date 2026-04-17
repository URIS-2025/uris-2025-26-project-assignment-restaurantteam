using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Payloads;
using OrderService.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OrderService.Handlers.Order;
using OrderService.Handlers.Links;
using OrderService.DTO.Order;
using OrderService.Handlers.OrderItem;
using OrderService.DTO.OrderItem;
using OrderService.DTO.MenuItem;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOrderHandler orderHandler;
        private readonly IMenuItemLink menuItemLink;
        private readonly IOrderItemHandler orderItemHandler;
        private readonly IUserLink userLink;

        public OrderController(IOrderHandler orderHandler,
                                IUserLink userLink,
                                IMenuItemLink menuItemLink,
                                IOrderItemHandler orderItemHandler)
        {
            this.orderHandler = orderHandler;
            this.userLink = userLink;
            this.menuItemLink = menuItemLink;
            this.orderItemHandler = orderItemHandler;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO dto, [FromHeader] string authorization)
        {
            var idUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (User.IsInRole("CUSTOMER") || User.IsInRole("EMPLOYEE"))
            {
                dto.IdUser = idUser;
                dto.OrderStatus = OrderStatus.PENDING;
                dto.CreatedAt = DateTime.UtcNow;
            }
            if(dto.IdUser == null)
            dto.IdUser = idUser;
            dto.OrderStatus = OrderStatus.PENDING;
            dto.CreatedAt = DateTime.Now;

            var userDTO = userLink.GetUserById(idUser, authorization).Result;
            if (userDTO == null)
            {
                return BadRequest("User doesn't exist");
            }

            var orderDTO = await orderHandler.CreateOrder(dto);
            orderDTO.UserDTO = userDTO;

            List<OrderItemDTO> orderItemDTOs = new List<OrderItemDTO>();
            decimal totalPrice = 0;
            foreach (CreateOrderItemDTO orderItem in dto.OrderItems)
            {
                var menuItemDTO = await menuItemLink.GetMenuItemById(orderItem.IdMenuItem, authorization);
                if (menuItemDTO == null)
                {
                    return BadRequest("Menu item doesn't exist");
                }
                totalPrice += menuItemDTO.Price * orderItem.Quantity;
                orderItem.PricePerItem = menuItemDTO.Price;
 
                var orderItemDTO = await orderItemHandler.CreateOrderItem(orderItem, orderDTO.IdOrder);
                orderItemDTO.MenuItemDTO = menuItemDTO;
                orderItemDTOs.Add(orderItemDTO);
            }

            orderDTO.OrderItems = orderItemDTOs;
            orderDTO.TotalPrice = totalPrice;
            var updatedOrderDTO = await orderHandler.UpdateOrder(new UpdateOrderDTO { TotalPrice = totalPrice }, orderDTO.IdOrder);

            return Ok(orderDTO);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders([FromHeader] string authorization)
        {
    

            var orderDTOs = await orderHandler.GetOrders();

            foreach(OrderDTO orderDTO in orderDTOs)
            {
                
                var userDTO = userLink.GetUserById(orderDTO.UserDTO.IdUser, authorization).Result;
                orderDTO.UserDTO = userDTO;
                foreach(OrderItemDTO orderItem in orderDTO.OrderItems)
                {
              


                     var menuItemDTO = menuItemLink.GetMenuItemById(orderItem.MenuItemDTO.IdMenuItem, authorization).Result;
                    if (menuItemDTO == null)
                    {
                        BadRequest("Menu item doesn't exist");
                    }
                    orderItem.MenuItemDTO = menuItemDTO;

                }
            }

            

            return Ok(orderDTOs);
        }

        [HttpGet("{idOrder}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById([FromHeader] string authorization, [FromRoute] int idOrder)
        {
            var orderDTO = await orderHandler.GetOrderById(idOrder);
            

            var userDTO = userLink.GetUserById(orderDTO.UserDTO.IdUser, authorization).Result;
            orderDTO.UserDTO = userDTO;

            foreach(OrderItemDTO orderItemDTO in orderDTO.OrderItems )
            {
                var menuItemDTO = await menuItemLink.GetMenuItemById(orderItemDTO.MenuItemDTO.IdMenuItem, authorization);
                orderItemDTO.MenuItemDTO = menuItemDTO;
            }
            return Ok(orderDTO);
        }


        [HttpPut("{idOrder}")]
        public async Task<ActionResult<OrderDTO>> UpdateOrder([FromHeader] string authorization, 
                                                                [FromRoute] int idOrder, 
                                                                [FromBody] UpdateOrderDTO updateOrderDTO)
        {
            var orderDTO = await orderHandler.UpdateOrder(updateOrderDTO,idOrder);
            
            if(updateOrderDTO.OrderItems != null && updateOrderDTO.OrderItems.Count != 0)
            {
                foreach (var orderItemDTO in orderDTO.OrderItems)
                {
                    await orderItemHandler.DeleteOrderItem(orderItemDTO.IdOrderItem);
                }
                foreach (CreateOrderItemDTO createOrderItemDTO in updateOrderDTO.OrderItems)
                {
                    await orderItemHandler.CreateOrderItem(createOrderItemDTO, idOrder);
                }
            }
            
            var userDTO = userLink.GetUserById(orderDTO.UserDTO.IdUser, authorization).Result;
            orderDTO.UserDTO = userDTO;

            foreach (OrderItemDTO orderItemDTO in orderDTO.OrderItems)
            {
                var menuItemDTO = await menuItemLink.GetMenuItemById(orderItemDTO.MenuItemDTO.IdMenuItem, authorization);
                orderItemDTO.MenuItemDTO = menuItemDTO;
            }
            return Ok(orderDTO);
        }

        [HttpPatch("{idOrder}/status")]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatus([FromHeader] string authorization,
                                                        [FromRoute] int idOrder,
                                                        [FromBody] UpdateOrderDTO updateOrderDTO)
        {
            var orderDTO = await orderHandler.UpdateOrderStatus(updateOrderDTO, idOrder);

            var userDTO = userLink.GetUserById(orderDTO.UserDTO.IdUser, authorization).Result;
            orderDTO.UserDTO = userDTO;

            foreach (OrderItemDTO orderItemDTO in orderDTO.OrderItems)
            {
                var menuItemDTO = await menuItemLink.GetMenuItemById(orderItemDTO.MenuItemDTO.IdMenuItem, authorization);
                orderItemDTO.MenuItemDTO = menuItemDTO;
            }
            return Ok(orderDTO);
        }

        [HttpGet("items/{idMenuItem}")]
        public async Task<ActionResult<OrderDTO>> IsMenuItemInUse( [FromRoute] int idMenuItem)
        {
            bool isInUse = await orderItemHandler.IsMenuItemInUse(idMenuItem);
            return Ok(isInUse);
        }

    }
}
