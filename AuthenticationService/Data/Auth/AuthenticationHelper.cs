using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService.Model;
using AuthenticationService.Model.Dto;
using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Data.Auth
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly string _jwtSecret;
        private readonly int _jwtLifespan; // u minutima

        public AuthenticationHelper(IConfiguration configuration)
        {
            _jwtSecret = configuration["Jwt:Secret"];
            _jwtLifespan = int.Parse(configuration["Jwt:LifespanMinutes"]);
        }
        //  Hashiranje password-a (jednostavno sa SHA256)
        public string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //  Verifikacija password-a
        public bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }

        //  Generisanje JWT tokena
        public LoginResponseDto GenerateJwtToken(AuthUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponseDto
            {
                IdUser = user.IdUser,
                Username = user.Username,
                Role = user.Role,
                Token = tokenString,
                ExpiresAt = tokenDescriptor.Expires.Value
            };
        }
    }
}