using URIS.Entities.Enums;

namespace AccountService.Entities
{
    public class UpdateUserDTO
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public UserRole? Role { get; set; }

        public int? IdAddress { get; set; }
    }
}
