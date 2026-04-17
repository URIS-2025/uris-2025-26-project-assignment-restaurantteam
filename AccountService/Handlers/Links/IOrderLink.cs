namespace AccountService.Handlers.Links
{
    public interface IOrderLink
    {
        public Task<bool?> IsUserInUse(int idUser, string? token);
    }
}
