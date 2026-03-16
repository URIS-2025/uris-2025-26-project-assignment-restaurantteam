namespace AuthenticationService.Entities
{
    public class LoginResponseDTO
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
