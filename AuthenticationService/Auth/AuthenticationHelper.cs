using AuthenticationService.Entities;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService.Auth
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan;
        private readonly string _jwtAudience;
        private readonly string _jwtIssuer;

        public AuthenticationHelper(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Secret"];
            _jwtLifespan = int.Parse(configuration["Jwt:LifespanMinutes"]);
            _jwtAudience = configuration["Jwt:Audience"];
            _jwtIssuer = configuration["Jwt:Issuer"];

        }

        public LoginResponseDTO GenerateJwtToken(User user)
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
                IdUser = user.IdUser,
                Username = user.Username,
                Role = user.Role.ToString(),
                Token = tokenString,
                ExpiresAt = tokenDescriptor.Expires.Value
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
    }
}
