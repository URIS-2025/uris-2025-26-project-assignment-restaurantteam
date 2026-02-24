using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AccountService.Entities.Enums;

namespace AccountService.Entities
{
    public class User
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Enums.UserRole Role { get; set; }

        public int? IdAddress { get; set; }
        public Address Address { get; set; }

        
    }
}