using System.ComponentModel.DataAnnotations;

namespace AccountService.Models
{
    public enum Role
    {
        CUSTOMER,
        EMPLOYEE,
        ADMIN
    }

    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public Role Role { get; set; }

        // Embedded Value Object
        public Address Address { get; set; }
    }
}
