using MenuService.Data;
using MenuService.DTO.Category;
using MenuService.DTO.MenuItem;
using MenuService.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MenuService.Handlers.MenuItemCategory
{
    public class MenuItemCategoryHandler : IMenuItemCategoryHandler
    {
        private readonly MenuDbContext _context;

        public MenuItemCategoryHandler(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDTO>> CreateMenuItemCategory(int idMenuItem, List<int> categoryIds)
        {

            foreach(int idCategory in categoryIds)
            {
                Entities.MenuItemCategory menuItemCategory = new Entities.MenuItemCategory()
                {
                    IdMenuItem = idMenuItem,
                    IdCategory = idCategory,
                };

                _context.MenuItemCategories.Add(menuItemCategory);
            }
            await _context.SaveChangesAsync();

            var categories = await _context.MenuItemCategories
                .Where(mc => mc.IdMenuItem == idMenuItem)
                .Include(mc => mc.Category)
                .Select(mc => mc.Category)
                .ToListAsync();

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();

            foreach (Entities.Category category in categories)
            {
                categoryDTOs.Add(CategoryMapper.ToCategoryDTO(category));
            }

            return categoryDTOs;
        }

        public async Task<List<CategoryDTO>> GetMenuItemCategories(int idMenuItem)
        {
            var categories = await _context.MenuItemCategories
                .Where(mc => mc.IdMenuItem == idMenuItem)
                .Include(mc => mc.Category)
                .Select(mc => mc.Category)
                .ToListAsync();

            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();

            foreach (Entities.Category category in categories)
            {
                categoryDTOs.Add(CategoryMapper.ToCategoryDTO(category));
            }

            return categoryDTOs;
        }


      /*  public async Task<List<CategoryDTO>> UpdateMenuItemCategories(int idMenuItem, List<int> categoryIds)
        {

        }*/


        public async Task<bool> DeleteMenuItemCategories(int idMenuItem)
        {

            var menuItem = await _context.MenuItems
                .Include(m => m.MenuItemCategories)
                .Include(m => m.MenuItemIngredients)
                .FirstOrDefaultAsync(m => m.IdMenuItem == idMenuItem);

            if (menuItem == null)
                throw new Exception("Not found!");


            _context.MenuItemCategories.RemoveRange(menuItem.MenuItemCategories);
            _context.MenuItemIngredients.RemoveRange(menuItem.MenuItemIngredients);
            return true;

        }
    }
}
