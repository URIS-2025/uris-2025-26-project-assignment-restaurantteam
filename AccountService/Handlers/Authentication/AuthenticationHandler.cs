using AccountService.Data;
using AccountService.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountService.Handlers.Authentication
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;
        private readonly string _jwtAudience;
        private readonly string _jwtIssuer;
        private readonly AccountDbContext _context;

        public AuthenticationHandler(IConfiguration configuration, AccountDbContext context)
        {
            _jwtSecret = configuration["Jwt:Secret"];
            _jwtLifespan = int.Parse(configuration["Jwt:LifespanMinutes"]);
            _jwtAudience = configuration["Jwt:Audience"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _context = context;

        }

        public LoginResponseDTO GenerateJwtToken(Entities.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role.ToString(), user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
                Audience = _jwtAudience,
                Issuer = _jwtIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponseDTO
            {
                Token = tokenString,
            };
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginRequest)
        {


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
            if (user == null) throw new Exception("User doesn't exist.");

            if (!VerifyPassword(loginRequest.Password, user.Password))
                throw new Exception("Wrong credentials.");

            var authUser = new Entities.User
            {
                IdUser = user.IdUser,
                Username = user.Username,
                Role = user.Role
            };

            var token = GenerateJwtToken(authUser);
            return token;
        }
    }
}
