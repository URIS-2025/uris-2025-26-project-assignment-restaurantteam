using AccountService.DTO.Address;
using System;
using System.ComponentModel.DataAnnotations;
using AccountService.Entities.Enums;

namespace AccountService.DTO.User
{
    public class UserDTO
    {
        [Key]
        public int IdUser { get; set; }

        [MaxLength(20, ErrorMessage = "Username must be below 20 characters.")]
        public string Username { get; set; }

        [MaxLength(50, ErrorMessage = "Email must be below 20 characters.")]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(15, ErrorMessage = "Phone number must be below 15 characters.")]
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; }
        public AddressDTO? Address { get; set; }
    }
}
