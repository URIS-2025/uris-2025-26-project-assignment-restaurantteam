using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Entities
{
    public class Address
    {
        public Address() { }

        [Key]
        public int idAddress { get; set; }
        [Required]
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
