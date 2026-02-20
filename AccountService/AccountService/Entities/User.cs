using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AccountService.Entities.Enums;

namespace AccountService.Entities
{
    public class User
    {
        public int IdUser { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserRole Role { get; private set; }

        public Address Address { get; private set; }

        private User() { }

        public User(string username, string email, string password, string phoneNumber)
        {
            Username = username;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Role = UserRole.CUSTOMER;
        }

        public void ChangeRole(UserRole role)
        {
            Role = role;
        }
    }
}