using AccountService.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.DTO.Users
{
    public class ChangeRoleDto
    {
        public UserRole Role { get; set; } // CUSTOMER, EMPLOYEE, ADMIN
    }
}