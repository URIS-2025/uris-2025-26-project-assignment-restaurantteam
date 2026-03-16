using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URIS.Models;
using URIS.Data;
using Microsoft.AspNetCore.Authorization;
using AccountService.Entities;

namespace URIS.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly AccountDbContext _context;

        public UserController(AccountDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }


        [HttpGet("claims")]
        [Authorize(Roles = "CUSTOMER")]
        public IActionResult Claims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            
            user.Username = dto.Username ?? user.Username;
            user.Email = dto.Email ?? user.Email;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
            user.Role = dto.Role ?? user.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
