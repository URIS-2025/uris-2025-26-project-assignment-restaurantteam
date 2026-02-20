using System.Linq;
using AccountService.Data;
using AccountService.DTO.Auth;
using AccountService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AccountDbContext _context;

        public AuthController(AccountDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return BadRequest("Email already exists");

            var user = new User(dto.Username, dto.Email, dto.Password, dto.PhoneNumber);
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new AuthResponseDto
            {
                UserId = user.IdUser,
                Role = user.Role.ToString(),
                Token = "JWT_PLACEHOLDER"
            });
        }
    }
}