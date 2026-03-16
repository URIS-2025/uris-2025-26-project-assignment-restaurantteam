using AuthenticationService.Entities;

namespace AuthenticationService.Auth
{
    public interface IAuthenticationHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
        LoginResponseDTO GenerateJwtToken(User user);
    }
}
