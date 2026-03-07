using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}