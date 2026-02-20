using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Models
{
    public class Address
    {
        public int IdAddress { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
    }
}
