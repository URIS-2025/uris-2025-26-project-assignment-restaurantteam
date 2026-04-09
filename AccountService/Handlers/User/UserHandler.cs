using AccountService.Data;
using AccountService.DTO;
using AccountService.DTO.Address;
using AccountService.DTO.User;
using AccountService.Entities;
using AccountService.Mappers;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using AccountService.Handlers.Address;

namespace AccountService.Handlers.User
{
    public class UserHandler : IUserHandler
    {
        private readonly AccountDbContext _context;

        public UserHandler(AccountDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> CreateUser(CreateUserDTO createUserDTO, int idAddress)
        {

            var existsUser = await _context.Users.FirstOrDefaultAsync(u =>
              u.Username == createUserDTO.Username
              );

            if (existsUser != null)
            {
                throw new Exception("User already exists.");
            }

            Entities.User user = UserMapper.ToUser(createUserDTO, idAddress);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, workFactor: 12);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            UserDTO userDTO = UserMapper.ToUserDTO(user);
            return userDTO;

        }

        public Task<bool> DeleteUser(int userID)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserById(int userID)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.IdUser == userID);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var userDTO = UserMapper.ToUserDTO(user);
            userDTO.Address = AddressMapper.ToAddressDTO(user.Address);

            return userDTO;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _context.Users
                    .Include(u => u.Address)
                    .ToListAsync();

            //var users = _context.Users.ToList();
            List<UserDTO> userDTOs = new List<UserDTO>();

            foreach(Entities.User u in users)
            {
                var userDTO = UserMapper.ToUserDTO(u);
                Console.WriteLine("Username is " + u.Username);
                Console.WriteLine("Address ID is " + u.IdAddress);

                Console.WriteLine("Address is " + u.Address.Country);

                userDTO.Address = AddressMapper.ToAddressDTO(u.Address);
                userDTOs.Add(userDTO);
                //var address = await _context.Addresses.FindAsync(u.IdAddress);
            }

            return userDTOs;
        }

        public async Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO, int idUser)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Username = updateUserDTO.Username;
            user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDTO.Password, workFactor: 12);
            user.Email = updateUserDTO.Email;
            user.PhoneNumber = updateUserDTO.PhoneNumber;
            user.Role = updateUserDTO.Role;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            UserDTO userDTO = UserMapper.ToUserDTO(user);
            AddressDTO addressDTO = AddressMapper.ToAddressDTO(user.Address);
            userDTO.Address = addressDTO;

            return userDTO;
        }

        public async Task<UserDTO> UpdateUserRole(UpdateUserRoleDTO updateUserRoleDTO, int idUser)
        {
            Entities.User? user = await _context.Users.FindAsync(idUser);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Role = updateUserRoleDTO.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            UserDTO userDTO = UserMapper.ToUserDTO(user);

            return userDTO;
        }
    }
}
