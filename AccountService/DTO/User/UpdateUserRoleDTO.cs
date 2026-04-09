using AccountService.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccountService.DTO.User
{
    public class UpdateUserRoleDTO
    {
        [Required]
        public UserRole Role { get; set; }
    }
}
