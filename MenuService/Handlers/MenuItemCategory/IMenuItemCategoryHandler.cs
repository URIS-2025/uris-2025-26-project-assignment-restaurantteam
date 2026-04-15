using MenuService.DTO.MenuItem;
using MenuService.DTO.Category;


namespace MenuService.Handlers.MenuItemCategory
{
    public interface IMenuItemCategoryHandler
    {
        public Task<List<CategoryDTO>> CreateMenuItemCategory(int idMenuItem, List<int> categoryIds);

        public Task<List<CategoryDTO>> GetMenuItemCategories(int idMenuItem);

        //public Task<List<CategoryDTO>> UpdateMenuItemCategories(int idMenuItem, List<int> categoryIds);

        public Task<bool> DeleteMenuItemCategories(int idMenuItem);

        /* public Task<List<MenuItemDTO>> GetMenuItems();
         public Task<MenuItemDTO> GetMenuItemById(int idMenuItem);
         public Task<MenuItemDTO> UpdateMenuItem(UpdateMenuItemDTO updateMenuItemDTO, int idMenuItem);
         public Task<bool> DeleteMenuItem(int idMenuItem);*/
    }
}
