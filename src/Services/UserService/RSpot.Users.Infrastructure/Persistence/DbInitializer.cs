using Microsoft.AspNetCore.Identity;
using RSpot.Users.Domain.Interfaces;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(
            UsersDbContext context,
            IPasswordHasher<User> passwordHasher,
            IUserRepository userRepository)
        {
            context.Database.EnsureCreated();

            var adminEmail = "admin@example.com";
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Email = adminEmail,
                    Name = "Admin",
                    Role = "Admin"
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

                userRepository.CreateAsync(admin).Wait();
            }
        }
    }
}
