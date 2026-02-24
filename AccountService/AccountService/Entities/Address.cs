using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Entities
{
    public class Address
    {
        public Address() { }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }       
    }
}