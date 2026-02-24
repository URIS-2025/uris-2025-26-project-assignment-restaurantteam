using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuService.Data;
using MenuService.DTO.MenuItem;
using MenuService.DTO.Category;
using MenuService.DTO.Ingredient;
using MenuService.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MenuService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public MenuController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemResponseDto>>> GetMenu()
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemCategories)
                    .ThenInclude(mc => mc.Category)
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .ToListAsync();

            return menuItems.Select(MapToResponseDto).ToList();
        }

        // GET: api/menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemResponseDto>> GetMenuItem(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemCategories)
                    .ThenInclude(mc => mc.Category)
                .Include(m => m.MenuItemIngredients)
                    .ThenInclude(mi => mi.Ingredient)
                .FirstOrDefaultAsync(m => m.IdMenuItem == id);

            if (menuItem == null)
                return NotFound();

            return MapToResponseDto(menuItem);
        }

        // POST: api/menu
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MenuItemResponseDto>> CreateMenuItem(CreateMenuItemDto dto)
        {
            var menuItem = new MenuItem
            {
                MenuItemName = dto.MenuItemName,
                Description = dto.Description,
                Price = dto.Price,
                Calories = dto.Calories,
                IsAvailable = dto.IsAvailable
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            // Categories
            if (dto.CategoryIds != null)
            {
                foreach (var categoryId in dto.CategoryIds)
                {
                    _context.MenuItemCategories.Add(new MenuItemCategory
                    {
                        IdMenuItem = menuItem.IdMenuItem,
                        IdCategory = categoryId
                    });
                }
            }

            // Ingredients
            if (dto.IngredientIds != null)
            {
                foreach (var ingredientId in dto.IngredientIds)
                {
                    _context.MenuItemIngredients.Add(new MenuItemIngredient
                    {
                        IdMenuItem = menuItem.IdMenuItem,
                        IdIngredient = ingredientId
                    });
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem),
                new { id = menuItem.IdMenuItem },
                MapToResponseDto(menuItem));
        }

        // PUT: api/menu/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItemDto dto)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemCategories)
                .Include(m => m.MenuItemIngredients)
                .FirstOrDefaultAsync(m => m.IdMenuItem == id);

            if (menuItem == null)
                return NotFound();

            menuItem.MenuItemName = dto.MenuItemName;
            menuItem.Description = dto.Description;
            menuItem.Price = dto.Price;
            menuItem.Calories = dto.Calories;
            menuItem.IsAvailable = dto.IsAvailable;

            // Clear old relations
            _context.MenuItemCategories.RemoveRange(menuItem.MenuItemCategories);
            _context.MenuItemIngredients.RemoveRange(menuItem.MenuItemIngredients);

            // Add new relations
            if (dto.CategoryIds != null)
            {
                foreach (var categoryId in dto.CategoryIds)
                {
                    _context.MenuItemCategories.Add(new MenuItemCategory
                    {
                        IdMenuItem = id,
                        IdCategory = categoryId
                    });
                }
            }

            if (dto.IngredientIds != null)
            {
                foreach (var ingredientId in dto.IngredientIds)
                {
                    _context.MenuItemIngredients.Add(new MenuItemIngredient
                    {
                        IdMenuItem = id,
                        IdIngredient = ingredientId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/menu/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // -----------------------------
        // Mapping helper
        // -----------------------------
        private static MenuItemResponseDto MapToResponseDto(MenuItem menuItem)
        {
            return new MenuItemResponseDto
            {
                IdMenuItem = menuItem.IdMenuItem,
                MenuItemName = menuItem.MenuItemName,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Calories = menuItem.Calories,
                IsAvailable = menuItem.IsAvailable,
                Categories = menuItem.MenuItemCategories?
                    .Select(mc => new CategoryDto
                    {
                        IdCategory = mc.Category.IdCategory,
                        CategoryName = mc.Category.CategoryName
                    }).ToList(),
                Ingredients = menuItem.MenuItemIngredients?
                    .Select(mi => new IngredientDto
                    {
                        IdIngredient = mi.Ingredient.IdIngredient,
                        IngredientName = mi.Ingredient.IngredientName,
                        IsAllergen = mi.Ingredient.IsAllergen
                    }).ToList()
            };
        }
    }
}