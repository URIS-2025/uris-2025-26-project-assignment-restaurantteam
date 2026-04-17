using MenuService.Controllers;
using MenuService.Data;
using MenuService.Entities;
using MenuService.Handlers.Category;
using MenuService.Handlers.Ingredient;
using MenuService.Handlers.Links;
using MenuService.Handlers.MenuItem;
using OrderService.Handlers.Order;
using OrderService.Data;
using OrderService.Handlers.Links;
using OrderService.Handlers.OrderItem;
using MenuService.Handlers.MenuItemCategory;
using MenuService.Handlers.MenuItemIngredient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using OrderService.Data;

namespace MenuService.Tests
{
    public class MenuControllerTest
    {

        private readonly ITestOutputHelper output;
        public MenuControllerTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        private MenuDbContext GetInMemoryMenuContext()
        {
            var options = new DbContextOptionsBuilder<MenuDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new MenuDbContext(options);
        }

        private MenuController GetMenuController()
        {
            var context = GetInMemoryMenuContext();

            var menuHandler = new MenuItemHandler(context);
            var menuItemCategoryHandler = new MenuItemCategoryHandler(context);
            var menuItemIngredientHandler = new MenuItemIngredientHandler(context);
            var orderLink = new OrderLink();

            var controller = new MenuController(context, menuHandler, menuItemCategoryHandler, menuItemIngredientHandler, orderLink);
            return controller;
        }

        private MenuController GetMenuController(MenuDbContext context)
        {
            var menuHandler = new MenuItemHandler(context);
            var menuItemCategoryHandler = new MenuItemCategoryHandler(context);
            var menuItemIngredientHandler = new MenuItemIngredientHandler(context);
            var orderLink = new OrderLink();

            var controller = new MenuController(context, menuHandler, menuItemCategoryHandler, menuItemIngredientHandler, orderLink);
            return controller;
        }
        private OrderDbContext GetInMemoryOrderContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new OrderDbContext(options);
        }

        private OrderController GetOrderController()
        {
            var contextOrder = GetInMemoryOrderContext();

            var orderHandler = new OrderHandler(contextOrder);
            var orderItemsHandler = new OrderItemHandler(contextOrder);
            var userLink = new UserLink();
            var menuLink = new MenuItemLink();

            var orderController = new OrderController(orderHandler, userLink, menuLink, orderItemsHandler);
            return orderController;
        }

        private OrderController GetOrderController(OrderDbContext context)
        {
            var orderHandler = new OrderHandler(context);
            var orderItemsHandler = new OrderItemHandler(context);
            var userLink = new UserLink();
            var menuLink = new MenuItemLink();

            var orderController = new OrderController(orderHandler, userLink, menuLink, orderItemsHandler);
            return orderController;
        }
        // Test 1 — GetMenu vraca sve stavke menija
        [Fact]
        public async Task GetMenu_ReturnsAllMenuItems()
        {
            var context = GetInMemoryMenuContext();

            context.MenuItems.AddRange(
                new MenuItem { MenuItemName = "Pizza", Price = 10.99m, Description = "Test opis", IsAvailable = true },
                new MenuItem { MenuItemName = "Burger", Price = 8.99m, Description = "Test opis", IsAvailable = true }
            );
            await context.SaveChangesAsync();

            var controller = GetMenuController(context);
            var result = await controller.GetMenuItems();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        // Test 2 — GetMenuItem vraca NotFound za nepostojeci ID
        [Fact]
        public async Task GetMenuItem_WithInvalidId_ReturnsNotFound()
        {
            var context = GetInMemoryMenuContext();
            var menuHandler = new MenuItemHandler(context);
            var menuItemCategoryHandler = new MenuItemCategoryHandler(context);
            var menuItemIngredientHandler = new MenuItemIngredientHandler(context);
            var orderLink = new OrderLink();
            var controller = new MenuController(context, menuHandler, menuItemCategoryHandler, menuItemIngredientHandler, orderLink);

            var result = await controller.GetMenuItemById(999);
            Console.WriteLine(result.GetType());
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 3 — GetMenuItem vraca ispravnu stavku
        [Fact]
        public async Task GetMenuItem_WithValidId_ReturnsMenuItem()
        {
            var context = GetInMemoryMenuContext();
            var menuHandler = new MenuItemHandler(context);
            var menuItemCategoryHandler = new MenuItemCategoryHandler(context);
            var menuItemIngredientHandler = new MenuItemIngredientHandler(context);
            var orderLink = new OrderLink();

            var item = new MenuItem { MenuItemName = "Pizza", Description = "Test opis", Price = 10.99m, IsAvailable = true };
            context.MenuItems.Add(item);
            await context.SaveChangesAsync();

            var controller = new MenuController(context, menuHandler, menuItemCategoryHandler, menuItemIngredientHandler, orderLink);
            var result = await controller.GetMenuItemById(item.IdMenuItem);

            Assert.NotNull(result.Value);
            Assert.Equal("Pizza", result.Value.MenuItemName);
        }

        // Test 4 — DeleteMenuItem vraca NotFound za nepostojeci ID
        [Fact]
        public async Task DeleteMenuItem_WithInvalidId_ReturnsNotFound()
        {
            var controller = GetMenuController();
            var orderController = GetOrderController();


            var result = await controller.DeleteMenu(999, "sss");
            output.WriteLine(result.GetType().FullName);
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // Test 5 — DeleteMenuItem uspjesno brise stavku
        [Fact]
        public async Task DeleteMenuItem_WithValidId_ReturnsNoContent()
        {
            var context = GetInMemoryMenuContext();
            var menuHandler = new MenuItemHandler(context);
            var menuItemCategoryHandler = new MenuItemCategoryHandler(context);
            var menuItemIngredientHandler = new MenuItemIngredientHandler(context);
            var orderLink = new OrderLink();

            var item = new MenuItem { MenuItemName = "Pizza", Description = "Test opis", Price = 10.99m, IsAvailable = true };
            context.MenuItems.Add(item);
            await context.SaveChangesAsync();

            var controller = new MenuController(context, menuHandler, menuItemCategoryHandler, menuItemIngredientHandler, orderLink);
            var result = await controller.DeleteMenu(item.IdMenuItem, "sss");

            Assert.IsType<NoContentResult>(result);
        }

        // Test 6 — GetCategories vraca sve kategorije
        [Fact]
        public async Task GetCategories_ReturnsAllCategories()
        {
            var context = GetInMemoryMenuContext();
            var categoryHandler = new CategoryHandler(context);
            context.Categories.AddRange(
                new Category { CategoryName = "Pizza" },
                new Category { CategoryName = "Pasta" }
            );
            await context.SaveChangesAsync();

            var controller = new CategoryController(categoryHandler);
            var result = await controller.GetCategories();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }

        // Test 7 — GetIngredients vraca sve sastojke
        [Fact]
        public async Task GetIngredients_ReturnsAllIngredients()
        {
            var context = GetInMemoryMenuContext();
            var ingredientHandler = new IngredientHandler(context);

            context.Ingredients.AddRange(
                new Ingredient { IngredientName = "Brasno", IsAllergen = false },
                new Ingredient { IngredientName = "Gluten", IsAllergen = true }
            );
            await context.SaveChangesAsync();

            var controller = new IngredientController(ingredientHandler);
            var result = await controller.GetIngredients();

            var okResult = result.Value;
            Assert.NotNull(okResult);
            Assert.Equal(2, okResult.Count());
        }
    }
}