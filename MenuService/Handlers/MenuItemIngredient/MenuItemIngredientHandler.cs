using MenuService.Data;
using MenuService.DTO.Category;
using MenuService.DTO.Ingredient;
using MenuService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Handlers.MenuItemIngredient
{
    public class MenuItemIngredientHandler : IMenuItemIngredientHandler
    {
        private readonly MenuDbContext _context;

        public MenuItemIngredientHandler(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<IngredientDTO>> CreateMenuItemIngredient(int idMenuItem, List<int> ingredientIds)
        {

            foreach (int idIngredient in ingredientIds)
            {
                Entities.MenuItemIngredient menuItemIngredient = new Entities.MenuItemIngredient()
                {
                    IdMenuItem = idMenuItem,
                    IdIngredient = idIngredient,
                };

                _context.MenuItemIngredients.Add(menuItemIngredient);
            }

            await _context.SaveChangesAsync();

            var ingredients = await _context.MenuItemIngredients
                .Where(mc => mc.IdMenuItem == idMenuItem)
                .Include(mc => mc.Ingredient)
                .Select(mc => mc.Ingredient)
                .ToListAsync();

            List<IngredientDTO> ingredientDTOs = new List<IngredientDTO>();

            foreach (Entities.Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(IngredientMapper.ToIngredientDTO(ingredient));
            }

            return ingredientDTOs;
        }

        public async Task<List<IngredientDTO>> GetMenuItemIngredients(int idMenuItem)
        {
            var ingredients = await _context.MenuItemIngredients
               .Where(mc => mc.IdMenuItem == idMenuItem)
               .Include(mc => mc.Ingredient)
               .Select(mc => mc.Ingredient)
               .ToListAsync();

            List<IngredientDTO> ingredientDTOs = new List<IngredientDTO>();

            foreach (Entities.Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(IngredientMapper.ToIngredientDTO(ingredient));
            }

            return ingredientDTOs;
        }


      /*  public async Task<bool> DeleteMenuItemIngredients(int idMenuItem)
        {

            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemIngredients)
                .Include(m => m.MenuItemIngredients)
                .FirstOrDefaultAsync(m => m.IdMenuItem == idMenuItem);

            if (menuItem == null)
                throw new Exception("Not found!");


            _context.MenuItemCategories.RemoveRange(menuItem.MenuItemCategories);
            _context.MenuItemIngredients.RemoveRange(menuItem.MenuItemIngredients);
            return true;

        }*/
    }
}
