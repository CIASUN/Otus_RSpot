namespace RSpot.Users.Application.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public UserResponse User { get; set; } = new();
    }
}
