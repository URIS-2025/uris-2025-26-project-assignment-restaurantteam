using System.Linq;

using MenuService.Data;
using MenuService.DTO.MenuItem;
using MenuService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public MenuController(MenuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMenu()
        {
            var items = _context.MenuItems
                .Where(m => m.IsAvailable)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetMenuItem(int id)
        {
            var item = _context.MenuItems.Find(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpGet("by-category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            var items = _context.MenuItems
                .Where(m => m.IsAvailable &&
                    m.Categories.Any(c => c.IdCategory == categoryId))
                .ToList();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult Create(CreateMenuItemDto dto)
        {
            var item = new MenuItem(
                dto.MenuItemName,
                dto.Description,
                dto.Price,
                dto.Calories
            );

            foreach (var categoryId in dto.CategoryIds)
            {
                item.Categories.Add(new MenuItemCategory
                {
                    IdCategory = categoryId
                });
            }

            foreach (var ingredientId in dto.IngredientIds)
            {
                item.Ingredients.Add(new MenuItemIngredient
                {
                    IdIngredient = ingredientId
                });
            }

            _context.MenuItems.Add(item);
            _context.SaveChanges();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateMenuItemDto dto)
        {
            var item = _context.MenuItems.Find(id);
            if (item == null) return NotFound();

            item.GetType().GetProperty("MenuItemName")?.SetValue(item, dto.MenuItemName);
            item.GetType().GetProperty("Description")?.SetValue(item, dto.Description);
            item.GetType().GetProperty("Price")?.SetValue(item, dto.Price);
            item.GetType().GetProperty("Calories")?.SetValue(item, dto.Calories);
            item.GetType().GetProperty("IsAvailable")?.SetValue(item, dto.IsAvailable);

            _context.SaveChanges();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public IActionResult Deactivate(int id)
        {
            var item = _context.MenuItems.Find(id);
            if (item == null) return NotFound();

            item.Deactivate();
            _context.SaveChanges();

            return Ok("Menu item deactivated");
        }
    }
}