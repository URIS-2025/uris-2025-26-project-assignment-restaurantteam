using AccountService.Entities;
using AccountService.DTO.User;

namespace AccountService.Handlers.User
{
    public interface IUserHandler
    {
        public Task<UserDTO> CreateUser(CreateUserDTO createUserDTOm , int idAddress);
        public Task<List<UserDTO>> GetUsers();
        public Task<UserDTO> GetUserById(int userID);
        public Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO, int idUser);
        public Task<UserDTO> UpdateUserRole(UpdateUserRoleDTO updateUserRoleDTO, int idUser);
        public Task<bool> DeleteUser(int userID);

    }
}
