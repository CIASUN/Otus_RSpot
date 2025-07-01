namespace RSpot.Users.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // хранить хэш, не пароль!
        public string FullName { get; set; }
        public string Role { get; set; } // "User", "Admin"
        public DateTime CreatedAt { get; set; }
    }
}
