using System;
using System.ComponentModel.DataAnnotations;

namespace URIS.Entities
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Enums.UserRole Role { get; set; }

        public int IdAddress { get; set; }
        public Address Address { get; set; }

    }
}
