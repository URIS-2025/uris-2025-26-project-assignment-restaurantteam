using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Entities
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Enums.UserRole Role { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
