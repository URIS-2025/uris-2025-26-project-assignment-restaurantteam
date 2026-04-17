namespace AccountService.Handlers.Links
{
    public interface IReservationLink
    {
        public Task<bool?> IsUserInUse(int idUser, string? token);
    }
}
