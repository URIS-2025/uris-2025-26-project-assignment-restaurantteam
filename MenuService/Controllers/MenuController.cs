using MenuService.Data;
using MenuService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Controllers
{
    [ApiController]
    [Route("api/menu")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public MenuController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetMenu()
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
        public async Task<ActionResult<MenuItemResponse>> GetMenuItem(int id)
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
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<ActionResult<MenuItemResponse>> CreateMenuItem(CreateMenuItem dto)
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

            Console.WriteLine("This is the CategorIES ", dto.CategoryIds[0]);
            
            // Categories
            if (dto.CategoryIds != null)
            {
                foreach (var categoryId in dto.CategoryIds)
                {
                    Console.WriteLine("This is the Category id ", categoryId);
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
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, UpdateMenuItem dto)
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
        [Authorize(Roles = "ADMIN")]
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
        private static MenuItemResponse MapToResponseDto(MenuItem menuItem)
        {
            return new MenuItemResponse
            {
                IdMenuItem = menuItem.IdMenuItem,
                MenuItemName = menuItem.MenuItemName,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Calories = menuItem.Calories,
                IsAvailable = menuItem.IsAvailable,
                Categories = menuItem.MenuItemCategories?
                    .Select(mc => new Category
                    {
                        IdCategory = mc.Category.IdCategory,
                        CategoryName = mc.Category.CategoryName
                    }).ToList(),
                Ingredients = menuItem.MenuItemIngredients?
                    .Select(mi => new Ingredient
                    {
                        IdIngredient = mi.Ingredient.IdIngredient,
                        IngredientName = mi.Ingredient.IngredientName,
                        IsAllergen = mi.Ingredient.IsAllergen
                    }).ToList()
            };
        }
        // ==============================
        // CATEGORY endpoints
        // ==============================

        // GET: api/menu/categories
        [HttpGet("categories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new Category
            {
                IdCategory = c.IdCategory,
                CategoryName = c.CategoryName
            }).ToList();
        }

        // POST: api/menu/categories
        [HttpPost("categories")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateCategory(CreateCategory dto)
        {
            var category = new Category { CategoryName = dto.CategoryName };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = category.IdCategory }, dto);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategory dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.IdCategory == id);

            if (category == null)
                return NotFound();

            category.CategoryName = dto.CategoryName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/menu/categories/{id}
        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==============================
        // INGREDIENT endpoints
        // ==============================

        // GET: api/menu/ingredients
        [HttpGet("ingredients")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return ingredients.Select(i => new Ingredient
            {
                IdIngredient = i.IdIngredient,
                IngredientName = i.IngredientName,
                IsAllergen = i.IsAllergen
            }).ToList();
        }

        // POST: api/menu/ingredients
        [HttpPost("ingredients")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateIngredient(CreateIngredient dto)
        {
            var ingredient = new Ingredient
            {
                IngredientName = dto.IngredientName,
                IsAllergen = dto.IsAllergen
            };
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIngredients), new { id = ingredient.IdIngredient }, dto);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, CreateIngredient dto)
        {
            var ingredient = await _context.Ingredients
                .FirstOrDefaultAsync(m => m.IdIngredient == id);

            if (ingredient == null)
                return NotFound();

            ingredient.IngredientName = dto.IngredientName;
            ingredient.IsAllergen = dto.IsAllergen;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/menu/ingredients/{id}
        [HttpDelete("ingredients/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return NotFound();
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}

