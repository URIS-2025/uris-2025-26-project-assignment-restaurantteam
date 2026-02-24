using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Model.Dto
{
    public class LoginResponseDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}