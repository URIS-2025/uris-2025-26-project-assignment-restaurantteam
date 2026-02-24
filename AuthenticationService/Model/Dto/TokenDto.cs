using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Model.Dto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}