using System.ComponentModel.DataAnnotations;

namespace AccountService.Entities
{
    public class Address
    {
        public Address() { }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; }

        [Range(1, 99999)]
        public int StreetNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }
    }
}