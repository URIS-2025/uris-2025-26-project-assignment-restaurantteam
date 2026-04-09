using System.ComponentModel.DataAnnotations;

namespace AccountService.DTO.Address
{
    public class UpdateAddressDTO
    {
        
        [MaxLength(30, ErrorMessage = "Street name has to be below 30 characters.")]
        public string Street { get; set; }

       
        [Range(1, 1000, ErrorMessage = "Street number can't be negative or above 1000")]
        public int StreetNumber { get; set; }

       
        [Range(1, 10000, ErrorMessage = "Street number can't be negative or above 10000")]
        public int PostalCode { get; set; }

       
        [MaxLength(30, ErrorMessage = "Country name has to be below 30 characters.")]
        public string Country { get; set; }

        
        [MaxLength(30, ErrorMessage = "City name has to be below 30 characters.")]
        public string City { get; set; }
    }
}
