using AccountService.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Handlers.Authentication
{
    public interface IAuthenticationHandler
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string storedHash);
        public LoginResponseDTO GenerateJwtToken(Entities.User user);
        public Task<LoginResponseDTO> Login(LoginDTO loginRequest);
    }
}
