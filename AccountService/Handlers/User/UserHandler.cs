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
                throw new Exception("User not found. USER SERVICE");
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

                userDTO.Address = AddressMapper.ToAddressDTO(u.Address);
                userDTOs.Add(userDTO);
                //var address = await _context.Addresses.FindAsync(u.IdAddress);
            }

            return userDTOs;
        }

        public async Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO, int idUser)
        {
            if (updateUserDTO == null)
                throw new Exception("Bad request");

            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if(updateUserDTO.Username !=  null)
                user.Username = updateUserDTO.Username;
            if (updateUserDTO.Password != null)
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDTO.Password, workFactor: 12);
            if (updateUserDTO.Email != null)
                user.Email = updateUserDTO.Email;
            if (updateUserDTO.PhoneNumber != null)
                user.PhoneNumber = updateUserDTO.PhoneNumber;
            if (updateUserDTO.Role != null)
                user.Role = updateUserDTO.Role.Value;
            
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
