using System.ComponentModel.DataAnnotations;

namespace AccountService.DTO.Authentication
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "User needs to have a username.")]
        [MaxLength(20, ErrorMessage = "Username must be below 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "User needs to have a password.")]
        [MaxLength(30, ErrorMessage = "Password must be below 30 characters.")]
        [MinLength(0, ErrorMessage = "Password must be above 4 characters.")]
        public string Password { get; set; }
    }
}
