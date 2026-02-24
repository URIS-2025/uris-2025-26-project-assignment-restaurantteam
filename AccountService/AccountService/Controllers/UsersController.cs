using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AccountService.Data;
using AccountService.DTO.Users;
using AccountService.Entities;
using AccountService.Entities.Enums;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AccountDbContext _context;

        public UsersController(AccountDbContext context)
        {
            _context = context;
        }

        // GET: api/users -> samo ADMIN vidi sve
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.Address).ToListAsync();
            return users.Select(u => MapToDto(u)).ToList();
        }

        // GET: api/users/{id} -> ADMIN ili sam korisnik
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _context.Users.Include(u => u.Address)
                                           .FirstOrDefaultAsync(u => u.IdUser == id);
            if (user == null) return NotFound();

            var userIdClaim = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roleClaim = User.FindFirst(ClaimTypes.Role).Value;

            if (userIdClaim != id && roleClaim != "ADMIN")
                return Forbid();

            return MapToDto(user);
        }

        // PUT: api/users/{id} -> update user
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateDto)
        {
            var user = await _context.Users.Include(u => u.Address)
                                           .FirstOrDefaultAsync(u => u.IdUser == id);
            if (user == null) return NotFound();

            var userIdClaim = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roleClaim = User.FindFirst(ClaimTypes.Role).Value;

            if (userIdClaim != id && roleClaim != "ADMIN")
                return Forbid();

            user.Username = updateDto.Username ?? user.Username;
            user.Email = updateDto.Email ?? user.Email;
            user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;

            if (updateDto.Address != null)
            {
                if (user.Address == null) user.Address = new Address();
                user.Address.Street = updateDto.Address.Street;
                user.Address.StreetNumber = updateDto.Address.StreetNumber;
                user.Address.PostalCode = updateDto.Address.PostalCode;
                user.Address.Country = updateDto.Address.Country;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/users/{id}/role -> samo ADMIN
        [HttpPut("{id}/role")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ChangeRole(int id, ChangeRoleDto roleDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Role = roleDto.Role;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/users/{id} -> samo ADMIN
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ==============================
        // Helper
        // ==============================
        private UserResponseDto MapToDto(User user)
        {
            return new UserResponseDto
            {
                IdUser = user.IdUser,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                Address = user.Address == null ? null : new AddressDto
                {
                    Street = user.Address.Street,
                    StreetNumber = user.Address.StreetNumber,
                    PostalCode = user.Address.PostalCode,
                    Country = user.Address.Country
                }
            };
        }
    }
}