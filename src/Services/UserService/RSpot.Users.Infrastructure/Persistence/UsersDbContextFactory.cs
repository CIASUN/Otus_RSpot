using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RSpot.Users.Infrastructure.Persistence;

namespace RSpot.Users.Infrastructure.Persistence
{
    public class UsersDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();

            // Вариант для PostgreSQL:
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=rspot;Username=postgres;Password=example");

            return new UsersDbContext(optionsBuilder.Options);
        }
    }
}
