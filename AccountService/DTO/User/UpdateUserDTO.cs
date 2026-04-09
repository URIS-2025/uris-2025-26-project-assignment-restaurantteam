using AccountService.DTO.Address;
using System.ComponentModel.DataAnnotations;
using AccountService.Entities.Enums;

namespace AccountService.DTO.User
{
    public class UpdateUserDTO
    {
        
        [MaxLength(20, ErrorMessage = "Username must be below 20 characters.")]
        public string? Username { get; set; }

        
        [MaxLength(20, ErrorMessage = "Email must be below 20 characters.")]
        [EmailAddress]
        public string? Email { get; set; }

        
        [MaxLength(30, ErrorMessage = "Password must be below 30 characters.")]
        [MinLength(5, ErrorMessage = "Password must be above 4 characters.")]
        public string? Password { get; set; }

        
        [MaxLength(15, ErrorMessage = "Phone number must be below 15 characters.")]
        public string? PhoneNumber { get; set; }

        
        public UserRole Role { get; set; }

        
        public UpdateAddressDTO? Address { get; set; }
    }
}
