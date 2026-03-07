using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Controllers;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Entities;
using OrderService.Entities.Enums;
using Xunit;

namespace OrderService.Tests
{
    public class OrderControllerTests
    {
        private OrderDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new OrderDbContext(options);
        }

        private void SetUserClaims(ControllerBase controller, int userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        // Test 1 — GetOrders vraca sve porudzbine za ADMIN
        [Fact]
        public async Task GetOrders_AsAdmin_ReturnsAllOrders()
        {
            var context = GetInMemoryContext();
            context.Orders.AddRange(
                new Order { IdUser = 1, OrderStatus = OrderStatus.PENDING, PaymentMethod = PaymentMethod.CASH, CreatedAt = DateTime.UtcNow, TotalPrice = 10.99m },
                new Order { IdUser = 2, OrderStatus = OrderStatus.PENDING, PaymentMethod = PaymentMethod.CASH, CreatedAt = DateTime.UtcNow, TotalPrice = 20.99m }
            );
            await context.SaveChangesAsync();

            var controller = new OrderController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsAssignableFrom<IEnumerable<OrderResponseDto>>(okResult.Value);
            Assert.Equal(2, orders.Count());
        }

        // Test 2 — GetOrders vraca samo svoje porudzbine za USER
        [Fact]
        public async Task GetOrders_AsUser_ReturnsOnlyOwnOrders()
        {
            var context = GetInMemoryContext();
            context.Orders.AddRange(
                new Order { IdUser = 1, OrderStatus = OrderStatus.PENDING, PaymentMethod = PaymentMethod.CASH, CreatedAt = DateTime.UtcNow, TotalPrice = 10.99m },
                new Order { IdUser = 2, OrderStatus = OrderStatus.PENDING, PaymentMethod = PaymentMethod.CASH, CreatedAt = DateTime.UtcNow, TotalPrice = 20.99m }
            );
            await context.SaveChangesAsync();

            var controller = new OrderController(context, null);
            SetUserClaims(controller, 1, "CUSTOMER");

            var result = await controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsAssignableFrom<IEnumerable<OrderResponseDto>>(okResult.Value);
            Assert.Single(orders);
        }

        // Test 3 — GetOrderById vraca NotFound za nepostojeci ID
        [Fact]
        public async Task GetOrderById_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new OrderController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetOrderById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test 4 — GetOrderById vraca Forbid za drugog korisnika
        [Fact]
        public async Task GetOrderById_AsOtherUser_ReturnsForbid()
        {
            var context = GetInMemoryContext();
            var order = new Order { IdUser = 1, OrderStatus = OrderStatus.PENDING, PaymentMethod = PaymentMethod.CASH, CreatedAt = DateTime.UtcNow, TotalPrice = 10.99m };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var controller = new OrderController(context, null);
            SetUserClaims(controller, 999, "CUSTOMER");

            var result = await controller.GetOrderById(order.IdOrder);

            Assert.IsType<ForbidResult>(result);
        }

        // Test 5 — UpdateOrderStatus vraca NotFound za nepostojeci ID
        [Fact]
        public async Task UpdateOrderStatus_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new OrderController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.UpdateOrderStatus(999, new UpdateOrderStatusDto { Status = OrderStatus.PREPARING });

            Assert.IsType<NotFoundResult>(result);
        }

        // Test 6 — DeleteOrder vraca NotFound za nepostojeci ID
        [Fact]
        public async Task DeleteOrder_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new OrderController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.DeleteOrder(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}