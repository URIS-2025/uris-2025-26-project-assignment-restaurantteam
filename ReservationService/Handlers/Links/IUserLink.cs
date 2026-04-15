using ReservationService.DTO.User;

namespace ReservationService.Handlers.Links
{
    public interface IUserLink
    {
        public Task<UserDTO> GetUserById(int idUser, string? token);
    }
}
