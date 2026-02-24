using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Data.Auth;
using AuthenticationService.Model;
using AuthenticationService.Model.Dto;
using System.Threading.Tasks;
using AuthenticationService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationHelper _authHelper;
        private readonly AuthenticationDbContext _context;

        public AuthenticationController(IAuthenticationHelper authHelper, AuthenticationDbContext context)
        {
            _authHelper = authHelper;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null) return Unauthorized("Korisnik ne postoji");

            if (!_authHelper.VerifyPassword(loginDto.Password, user.Password))
                return Unauthorized("Pogrešna lozinka");

            var authUser = new AuthUser
            {
                IdUser = user.IdUser,
                Username = user.Username,
                Role = user.Role.ToString()
            };

            var token = _authHelper.GenerateJwtToken(authUser);
            return Ok(token);
        }
    }
}