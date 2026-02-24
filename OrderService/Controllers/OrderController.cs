using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Entities;
using OrderService.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize] // SVI endpointi zahtevaju JWT
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrderController(OrderDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        // ADMIN vidi sve, USER vidi samo svoje porudžbine
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var orders = role == "ADMIN"
                ? await _context.Orders
                    .Include(o => o.OrderItems)
                    .ToListAsync()
                : await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.IdUser == userId)
                    .ToListAsync();

            var response = orders.Select(o => new OrderResponseDto
            {
                IdOrder = o.IdOrder,
                IdUser = o.IdUser,
                OrderStatus = o.OrderStatus,
                PaymentMethod = o.PaymentMethod,
                TotalPrice = o.TotalPrice,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(i => new OrderItemDto
                {
                    IdMenuItem = i.IdMenuItem,
                    Quantity = i.Quantity,
                    PricePerItem = i.PricePerItem
                }).ToList()
            });

            return Ok(response);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.IdOrder == id);

            if (order == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (order.IdUser != userId && role != "ADMIN")
                return Forbid();

            var response = new OrderResponseDto
            {
                IdOrder = order.IdOrder,
                IdUser = order.IdUser,
                OrderStatus = order.OrderStatus,
                PaymentMethod = order.PaymentMethod,
                TotalPrice = order.TotalPrice,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    IdMenuItem = i.IdMenuItem,
                    Quantity = i.Quantity,
                    PricePerItem = i.PricePerItem
                }).ToList()
            };

            return Ok(response);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var order = new Order
            {
                IdUser = userId,
                OrderStatus = OrderStatus.PENDING,
                PaymentMethod = dto.PaymentMethod,
                CreatedAt = DateTime.UtcNow,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    IdMenuItem = i.IdMenuItem,
                    Quantity = i.Quantity,
                    PricePerItem = i.PricePerItem
                }).ToList()
            };

            order.TotalPrice = order.OrderItems.Sum(i => i.Quantity * i.PricePerItem);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.IdOrder }, order.IdOrder);
        }

        // PUT: api/orders/{id}/status
        // SAMO ADMIN može menjati status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.OrderStatus = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //  DELETE: api/orders/{id}
        // ADMIN ili vlasnik porudžbine
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (order.IdUser != userId && role != "ADMIN")
                return Forbid();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}