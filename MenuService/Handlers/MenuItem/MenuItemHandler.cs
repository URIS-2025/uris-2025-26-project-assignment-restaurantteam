using MenuService.Data;
using MenuService.DTO.MenuItem;
using Microsoft.EntityFrameworkCore;
using MenuService.Mappers;

namespace MenuService.Handlers.MenuItem
{
    public class MenuItemHandler : IMenuItemHandler
    {
        private readonly MenuDbContext _context;

        public MenuItemHandler(MenuDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItemDTO> CreateMenuItem(CreateMenuItemDTO createMenuItemDTO)
        {
            var existMenuItem = await _context.MenuItems.FirstOrDefaultAsync(u =>
               u.MenuItemName == createMenuItemDTO.MenuItemName
            );

            if (existMenuItem != null)
                throw new Exception("Menu item exists!");

            var menuItem = MenuItemMapper.ToMenuItem(createMenuItemDTO);

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            var menuItemtDTO = MenuItemMapper.ToMenuItemDTO(menuItem);
            //menuItemtDTO.IdMenuItem = menuItem.IdMenuItem;

            return menuItemtDTO;
        }

        public async Task<bool> DeleteMenuItem(int idMenuItem)
        {
            Entities.MenuItem? menuItem = await _context.MenuItems.FindAsync(idMenuItem);

            if (idMenuItem == null)
            {
                throw new Exception("Menu item not found");
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<List<MenuItemDTO>> GetMenuItems()
        {
            return _context.MenuItems
                .Select(MenuItemMapper.ToMenuItemDTO)
                .ToList();
        }

        public async Task<MenuItemDTO> GetMenuItemById(int idMenuItem)
        {
            Entities.MenuItem? menuItem = await _context.MenuItems.FindAsync(idMenuItem);


            if (menuItem == null)
            {
                throw new Exception("Menu item not found");

            }

            return MenuItemMapper.ToMenuItemDTO(menuItem);
        }

        public async Task<MenuItemDTO> UpdateMenuItem(UpdateMenuItemDTO updateMenuItemDTO, int idMenuItem)
        {
            Entities.MenuItem? menuItem = await _context.MenuItems.FindAsync(idMenuItem);

            if (menuItem == null)
            {
                throw new Exception("Menu item not found");
            }

            menuItem.MenuItemName = updateMenuItemDTO.MenuItemName;
            menuItem.Description = updateMenuItemDTO.Description;
            menuItem.Price = updateMenuItemDTO.Price;
            menuItem.Calories = updateMenuItemDTO.Calories;
            menuItem.IsAvailable = updateMenuItemDTO.IsAvailable;


            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();

            MenuItemDTO menuItemDTO = MenuItemMapper.ToMenuItemDTO(menuItem);

            return menuItemDTO;
        }
    }
}
