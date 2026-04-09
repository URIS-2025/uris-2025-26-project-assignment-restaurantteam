using AccountService.DTO.User;
using AccountService.Entities;

namespace AccountService.Mappers
{
    public class UserMapper
    {
        public static User ToUser(UserDTO userDTO)
        {
            return new User
            {
                IdUser = userDTO.IdUser,
                Username = userDTO.Username,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                Role = userDTO.Role,
                IdAddress = userDTO.Address.IdAddress
            };
        }

        public static User ToUser(CreateUserDTO createUserDTO, int idAddress)
        {
            return new User
            {
                Username = createUserDTO.Username,
                Password = createUserDTO.Password,
                PhoneNumber = createUserDTO.PhoneNumber,
                Email = createUserDTO.Email,
                Role = createUserDTO.Role,
                IdAddress = idAddress
            };
        }

        public static User ToUser(UpdateUserDTO updateUserDTO, int idAddress)
        {
            return new User
            {
                Username = updateUserDTO.Username,
                Password = updateUserDTO.Password,
                PhoneNumber = updateUserDTO.PhoneNumber,
                Email = updateUserDTO.Email,
                Role = updateUserDTO.Role,
                IdAddress = idAddress
            };
        }

        public static UserDTO ToUserDTO(User user)
        {
            return new UserDTO
            {
                IdUser = user.IdUser,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Role = user.Role,
            };
        }
    }
}
