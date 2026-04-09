using System.ComponentModel.DataAnnotations;

namespace AccountService.Entities
{
    public class Address
    {
        public Address() { }

        [Key]
        public int IdAddress { get; set; }

        [Required(ErrorMessage = "Address has to have a street name.")]
        [MaxLength(30, ErrorMessage = "Street name has to be below 30 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Address has to have a street number.")]
        [Range(1,1000, ErrorMessage = "Street number can't be negative or above 1000")]
        public int StreetNumber { get; set; }

        [Required(ErrorMessage = "Address has to have a postal code.")]
        [Range(1, 10000, ErrorMessage = "Postal code can't be negative or above 10000")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "Address has to have a country name.")]
        [MaxLength(30, ErrorMessage = "Country name has to be below 30 characters.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Address has to have a city name.")]
        [MaxLength(30, ErrorMessage = "City name has to be below 30 characters.")]
        public string City { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
