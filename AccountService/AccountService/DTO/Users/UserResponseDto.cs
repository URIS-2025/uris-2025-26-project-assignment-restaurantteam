using AccountService.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.DTO.Users
{
    public class UserResponseDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public AddressDto Address { get; set; }
    }

    public class AddressDto
    {
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
    }
}