using RSpot.Users.Domain.Models;
using Microsoft.EntityFrameworkCore;
using RSpot.Users.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace RSpot.Users.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(UsersDbContext context)
    {
        var email = "admin@example.com";
        var password = "Admin123!";

        var exists = await context.Users.AnyAsync(u => u.Email == email);
        if (!exists)
        {
            var user = new User
            {
                Email = email,
                Name = "Admin",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, password);

            context.Users.Add(user);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Admin user seeded");
        }
        else
        {
            Console.WriteLine("ℹ️ Admin user already exists");
        }
    }
}
