using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Entities
{
    public class Address
    {
        public string Street { get; private set; }
        public int StreetNumber { get; private set; }
        public int PostalCode { get; private set; }
        public string Country { get; private set; }

        private Address() { }

        public Address(string street, int streetNumber, int postalCode, string country)
        {
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            Country = country;
        }
    }
}