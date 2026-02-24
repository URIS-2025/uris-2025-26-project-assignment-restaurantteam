using AuthenticationService.Model;
using AuthenticationService.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Data.Auth
{
    public interface IAuthenticationHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
        LoginResponseDto GenerateJwtToken(AuthUser user);
    }
}