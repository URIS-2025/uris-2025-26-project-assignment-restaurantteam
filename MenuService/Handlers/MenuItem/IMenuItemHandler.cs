using MenuService.DTO.MenuItem;

namespace MenuService.Handlers.MenuItem
{
    public interface IMenuItemHandler
    {
        public Task<MenuItemDTO> CreateMenuItem(CreateMenuItemDTO createMenuItemDTO);
        public Task<List<MenuItemDTO>> GetMenuItems();
        public Task<MenuItemDTO> GetMenuItemById(int idMenuItem);
        public Task<MenuItemDTO> UpdateMenuItem(UpdateMenuItemDTO updateMenuItemDTO, int idMenuItem);
        public Task<bool> DeleteMenuItem(int idMenuItem);
    }
}
