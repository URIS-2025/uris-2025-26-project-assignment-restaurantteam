using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}