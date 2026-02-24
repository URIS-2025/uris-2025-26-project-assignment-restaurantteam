using AuthenticationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Data.User
{
    interface IAuthUserRepository
    {
        Task<AuthUser> GetUserByUsernameAsync(string username);
        Task AddUserAsync(AuthUser user);
    }
}
