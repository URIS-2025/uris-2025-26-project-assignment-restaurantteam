using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.DTO.Users
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
    }
}