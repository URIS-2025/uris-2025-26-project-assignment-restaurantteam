using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenuService.Controllers;
using MenuService.Data;
using MenuService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MenuService.Tests
{
    public class MenuControllerTests
    {
        private MenuDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MenuDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new MenuDbContext(options);
        }

        // Test 1 — GetMenu vraca sve stavke menija
        [Fact]
        public async Task GetMenu_ReturnsAllMenuItems()
        {
            var context = GetInMemoryContext();
            context.MenuItems.AddRange(
                new MenuItem { MenuItemName = "Pizza", Price = 10.99m, IsAvailable = true },
                new MenuItem { MenuItemName = "Burger", Price = 8.99m, IsAvailable = true }
            );
            await context.SaveChangesAsync();

            var controller = new MenuController(context);
            var result = await controller.GetMenu();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        // Test 2 — GetMenuItem vraca NotFound za nepostojeci ID
        [Fact]
        public async Task GetMenuItem_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new MenuController(context);

            var result = await controller.GetMenuItem(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test 3 — GetMenuItem vraca ispravnu stavku
        [Fact]
        public async Task GetMenuItem_WithValidId_ReturnsMenuItem()
        {
            var context = GetInMemoryContext();
            var item = new MenuItem { MenuItemName = "Pizza", Price = 10.99m, IsAvailable = true };
            context.MenuItems.Add(item);
            await context.SaveChangesAsync();

            var controller = new MenuController(context);
            var result = await controller.GetMenuItem(item.IdMenuItem);

            Assert.NotNull(result.Value);
            Assert.Equal("Pizza", result.Value.MenuItemName);
        }

        // Test 4 — DeleteMenuItem vraca NotFound za nepostojeci ID
        [Fact]
        public async Task DeleteMenuItem_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryContext();
            var controller = new MenuController(context);

            var result = await controller.DeleteMenuItem(999);

            Assert.IsType<NotFoundResult>(result);
        }

        // Test 5 — DeleteMenuItem uspjesno brise stavku
        [Fact]
        public async Task DeleteMenuItem_WithValidId_ReturnsNoContent()
        {
            var context = GetInMemoryContext();
            var item = new MenuItem { MenuItemName = "Pizza", Price = 10.99m, IsAvailable = true };
            context.MenuItems.Add(item);
            await context.SaveChangesAsync();

            var controller = new MenuController(context);
            var result = await controller.DeleteMenuItem(item.IdMenuItem);

            Assert.IsType<NoContentResult>(result);
        }

        // Test 6 — GetCategories vraca sve kategorije
        [Fact]
        public async Task GetCategories_ReturnsAllCategories()
        {
            var context = GetInMemoryContext();
            context.Categories.AddRange(
                new Category { CategoryName = "Pizza" },
                new Category { CategoryName = "Pasta" }
            );
            await context.SaveChangesAsync();

            var controller = new MenuController(context);
            var result = await controller.GetCategories();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        // Test 7 — GetIngredients vraca sve sastojke
        [Fact]
        public async Task GetIngredients_ReturnsAllIngredients()
        {
            var context = GetInMemoryContext();
            context.Ingredients.AddRange(
                new Ingredient { IngredientName = "Brasno", IsAllergen = false },
                new Ingredient { IngredientName = "Gluten", IsAllergen = true }
            );
            await context.SaveChangesAsync();

            var controller = new MenuController(context);
            var result = await controller.GetIngredients();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }
    }
}