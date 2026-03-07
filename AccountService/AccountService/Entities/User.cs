using System.ComponentModel.DataAnnotations;
using AccountService.Entities.Enums;

namespace AccountService.Entities
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public int? IdAddress { get; set; }
        public Address Address { get; set; }
    }
}