using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model
{
    public class AuthUser
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
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}