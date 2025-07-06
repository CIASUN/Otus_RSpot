using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task<bool> ExistsByEmailAsync(string email);
        Task SaveChangesAsync();
        Task CreateAsync(User user);
    }
}
