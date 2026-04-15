using MenuService.DTO.MenuItem;
using MenuService.Entities;

namespace MenuService.Mappers
{
    public class MenuItemMapper
    {
        public static Entities.MenuItem ToMenuItem(MenuItemDTO menuItemDTO)
        {
            return new Entities.MenuItem
            {
                IdMenuItem = menuItemDTO.IdMenuItem,
                MenuItemName = menuItemDTO.MenuItemName,
                Price = menuItemDTO.Price,
                Description = menuItemDTO.Description,
                Calories = menuItemDTO.Calories,
                IsAvailable = menuItemDTO.IsAvailable,
            };

        }
        public static MenuItemDTO ToMenuItemDTO(Entities.MenuItem menuItem)
        {
            return new MenuItemDTO
            {
                IdMenuItem = menuItem.IdMenuItem,
                MenuItemName = menuItem.MenuItemName,
                Price = menuItem.Price,
                Description = menuItem.Description,
                Calories = menuItem.Calories,
                IsAvailable = menuItem.IsAvailable,
            };

        }

        public static MenuItemDTO ToMenuItemDTO(CreateMenuItemDTO createMenuItemDTO)
        {
            return new MenuItemDTO
            {
                MenuItemName = createMenuItemDTO.MenuItemName,
                Price = createMenuItemDTO.Price,
                Description = createMenuItemDTO.Description,
                Calories = createMenuItemDTO.Calories,
                IsAvailable = createMenuItemDTO.IsAvailable,
            };

        }

        public static Entities.MenuItem ToMenuItem(CreateMenuItemDTO createMenuItemDTO)
        {
            return new Entities.MenuItem
            {
                MenuItemName = createMenuItemDTO.MenuItemName,
                Price = createMenuItemDTO.Price,
                Description = createMenuItemDTO.Description,
                Calories = createMenuItemDTO.Calories,
                IsAvailable = createMenuItemDTO.IsAvailable
            };

        }
    }
}
