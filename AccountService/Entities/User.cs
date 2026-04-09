using System;
using System.ComponentModel.DataAnnotations;

namespace AccountService.Entities
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "User needs to have a username.")]
        [MaxLength(20, ErrorMessage = "Username must be below 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "User needs to have an email.")]
        [MaxLength(20, ErrorMessage = "Email must be below 20 characters.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "User needs to have a password.")]
        [MaxLength(150, ErrorMessage = "Password must be below 30 characters.")]
        [MinLength(0, ErrorMessage = "Password must be above 4 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User needs to have a phone number.")]
        [MaxLength(15, ErrorMessage = "Phone number must be below 15 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "User needs to have a role (CUSTOMER, EMPLOYEE or ADMIN.")]
        public Enums.UserRole Role { get; set; }

        [Required(ErrorMessage = "User needs to have an address.")]
        public int IdAddress { get; set; }

        public Address Address { get; set; }
    }
}
