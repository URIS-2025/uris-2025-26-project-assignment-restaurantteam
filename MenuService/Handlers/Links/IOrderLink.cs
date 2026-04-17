namespace MenuService.Handlers.Links
{
    public interface IOrderLink
    {
        public Task<bool?> IsMenuItemInUse(int idMenuItem, string? token);
    }
}
