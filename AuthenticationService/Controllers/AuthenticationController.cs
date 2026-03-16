using AuthenticationService.Auth;
using AuthenticationService.Data;
using AuthenticationService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationService.Controllers
{
  
    
        [Route("api/[controller]")]
        [ApiController]
        public class UserController : ControllerBase
        {
            private readonly IAuthenticationHelper _authHelper;
            private readonly AuthenticationDbContext _context;
            

            public UserController(IAuthenticationHelper authHelper, AuthenticationDbContext context)
            {
                _authHelper = authHelper;
                _context = context;
            }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("API is alive");

        [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
            {
            
            var existsAddress = await _context.Addresss.FirstOrDefaultAsync(u =>
                    u.City == dto.City &&
                    u.Country == dto.Country &&
                    u.Street == dto.Street &&
                    u.StreetNumber == dto.StreetNumber
                );
                var passedAddress = existsAddress;

                var address = new Address
                {
                    City = dto.City,
                    Country = dto.Country,
                    Street = dto.Street,
                    StreetNumber = dto.StreetNumber,
                    PostalCode = dto.PostalCode,
                };

                if (existsAddress == null)
                {
                
                _context.Addresss.Add(address);
                    
                    await _context.SaveChangesAsync();
                    passedAddress = address;
                }

            

            var exists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
                if (exists) return Conflict("Korisnik već postoji");
                var user = new User
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = _authHelper.HashPassword(dto.Password),
                    PhoneNumber = dto.PhoneNumber,
                    Role = dto.Role,
                    IdAddress = passedAddress.idAddress
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("Registracija uspješna");
            }

            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequest loginRequest)
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
                if (user == null) return Unauthorized("Korisnik ne postoji");

                if (!_authHelper.VerifyPassword(loginRequest.Password, user.Password))
                    return Unauthorized("Pogrešna lozinka");

                var authUser = new User
                {
                    IdUser = user.IdUser,
                    Username = user.Username,
                    Role = user.Role
                };

                var token = _authHelper.GenerateJwtToken(authUser);
                return Ok(token);
            }
        }
    }

