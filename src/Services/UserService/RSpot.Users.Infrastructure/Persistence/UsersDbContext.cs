using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RSpot.Users.Domain.Interfaces;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Infrastructure.Persistence
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
    }

    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.FindAsync(id);

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email == email);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }

}
