using OrderService.DTO.Order;
using OrderService.DTO.User;

namespace OrderService.Handlers.Links
{
    public interface IUserLink
    {
        public Task<bool> CheckUserId(int idUser);
        public Task<UserDTO> GetUserById(int idUser, string? token);

    }
}
