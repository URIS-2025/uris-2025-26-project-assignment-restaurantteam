using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Model.Dto
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
