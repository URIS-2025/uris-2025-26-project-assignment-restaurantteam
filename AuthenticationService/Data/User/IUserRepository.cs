using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Data.User
{
    public interface IUserRepository
    {
        public bool UserWithCredentialsExists(string username, string password);
    }
}