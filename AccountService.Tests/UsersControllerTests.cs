using System.Threading.Tasks;
using AccountService.Controllers;
using AccountService.Data;
using AccountService.Entities;
using AccountService.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace AccountService.Tests
{
    public class UsersControllerTests
    {
        private AccountDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AccountDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new AccountDbContext(options);
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

        // Test 1 — GetUsers vraca sve korisnike
        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            var context = GetInMemoryContext();
            context.Users.AddRange(
                new User { Username = "user1", Email = "user1@test.com", Password = "hash1", Role = UserRole.CUSTOMER },
                new User { Username = "user2", Email = "user2@test.com", Password = "hash2", Role = UserRole.CUSTOMER }
            );
            await context.SaveChangesAsync();

            var controller = new UsersController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetUsers();

            var okResult = Assert.IsType<ActionResult<IEnumerable<AccountService.DTO.Users.UserResponseDto>>>(result);
            Assert.NotNull(okResult.Value);
        }

        // Test 2 — GetUser vraca NotFound za nepostojeci ID
        [Fact]
        public async Task GetUser_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new UsersController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.GetUser(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test 3 — GetUser vraca korisnika za validan ID (ADMIN)
        [Fact]
        public async Task GetUser_WithValidId_AsAdmin_ReturnsUser()
        {
            var context = GetInMemoryContext();
            var user = new User { Username = "testuser", Email = "test@test.com", Password = "hash", Role = UserRole.CUSTOMER };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context, null);
            SetUserClaims(controller, 99, "ADMIN");

            var result = await controller.GetUser(user.IdUser);

            Assert.NotNull(result.Value);
            Assert.Equal("testuser", result.Value.Username);
        }

        // Test 4 — GetUser vraca Forbid ako nije ADMIN ni vlasnik
        [Fact]
        public async Task GetUser_AsOtherUser_ReturnsForbid()
        {
            var context = GetInMemoryContext();
            var user = new User { Username = "testuser", Email = "test@test.com", Password = "hash", Role = UserRole.CUSTOMER };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context, null);
            SetUserClaims(controller, 999, "CUSTOMER"); // drugi korisnik

            var result = await controller.GetUser(user.IdUser);

            Assert.IsType<ForbidResult>(result.Result);
        }

        // Test 5 — DeleteUser vraca NotFound za nepostojeci ID
        [Fact]
        public async Task DeleteUser_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new UsersController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.DeleteUser(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test 6 — DeleteUser uspjesno brise korisnika
        [Fact]
        public async Task DeleteUser_WithValidId_ReturnsNoContent()
        {
            var context = GetInMemoryContext();
            var user = new User { Username = "testuser", Email = "test@test.com", Password = "hash", Role = UserRole.CUSTOMER };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = new UsersController(context, null);
            SetUserClaims(controller, 1, "ADMIN");

            var result = await controller.DeleteUser(user.IdUser);

            Assert.IsType<NoContentResult>(result);
        }
    }
}