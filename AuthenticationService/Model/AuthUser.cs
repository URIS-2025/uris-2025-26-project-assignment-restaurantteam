using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Model
{
    public class AuthUser
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // CUSTOMER, EMPLOYEE, ADMIN

        // Po potrebi, možete dodati email ili phone za login
        public string Email { get; set; }
    }
}