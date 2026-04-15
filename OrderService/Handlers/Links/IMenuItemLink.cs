using OrderService.DTO.MenuItem;

namespace OrderService.Handlers.Links
{
    public interface IMenuItemLink
    {
        public Task<MenuItemDTO> GetMenuItemById(int idMenuItem, string? token);
    }
}
