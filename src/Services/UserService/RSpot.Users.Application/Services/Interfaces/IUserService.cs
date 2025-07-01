using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSpot.Users.Application.DTOs;

namespace RSpot.Users.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<UserResponse?> GetCurrentUserAsync(Guid userId);
    }
}
