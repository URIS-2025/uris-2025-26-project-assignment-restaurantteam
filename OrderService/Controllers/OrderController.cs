using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Entities;
using System;
using System.Linq;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrderController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                .ToList();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.IdOrder == id);

            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderDto dto)
        {
            var paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), dto.PaymentMethod);
            var order = new Order(dto.IdUser, paymentMethod);

            foreach (var item in dto.Items)
            {
                order.AddItem(item.IdMenuItem, item.Quantity, item.PricePerItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            var status = (OrderStatus)Enum.Parse(typeof(OrderStatus), dto.Status);
            order.UpdateStatus(status);

            _context.SaveChanges();
            return Ok(order);
        }
    }
}