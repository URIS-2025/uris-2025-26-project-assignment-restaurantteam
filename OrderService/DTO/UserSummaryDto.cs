using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.DTO
{
    public class UserSummaryDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public int? StreetNumber { get; set; }
    }
}