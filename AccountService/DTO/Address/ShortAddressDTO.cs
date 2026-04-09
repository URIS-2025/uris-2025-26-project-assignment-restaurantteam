using System.ComponentModel.DataAnnotations;

namespace AccountService.DTO.Address
{
    public class ShortAddressDTO
    {
        [Required(ErrorMessage = "Address has to have an id.")]
        public int IdAddress { get; set; }
        [Required(ErrorMessage = "Address has to have a street name.")]
        [MaxLength(30, ErrorMessage = "Street name has to be below 30 characters.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Address has to have a street number.")]
        [Range(1, 1000, ErrorMessage = "Street number can't be negative or above 1000")]
        public int StreetNumber { get; set; }
    }
}
