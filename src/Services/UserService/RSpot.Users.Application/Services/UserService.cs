using Microsoft.AspNetCore.Identity;
using RSpot.Users.Application.DTOs;
using RSpot.Users.Application.Services.Interfaces;
using RSpot.Users.Domain.Interfaces;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IPasswordHasher<User> _hasher;

        public UserService(
            IUserRepository repository,
            IJwtTokenGenerator tokenGenerator,
            IPasswordHasher<User> hasher)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _hasher = hasher;
        }

        public async Task<UserResponse?> GetCurrentUserAsync(Guid userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user is null) return null;

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role
            };
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            if (await _repository.ExistsByEmailAsync(request.Email))
                return null;

            var user = new User
            {
                Email = request.Email,
                Name = request.FullName
            };
            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _repository.GetByEmailAsync(request.Email);
            if (user is null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            return BuildAuthResponse(user);
        }

        /*----------------------------------------------------------------*/
        /*   Локальный хелпер                                             */
        /*----------------------------------------------------------------*/
        private AuthResponse BuildAuthResponse(User user)
        {
            string token = _tokenGenerator.GenerateToken(user);
            DateTime expires = _tokenGenerator.GetExpiration(token);

            return new AuthResponse
            {
                Token = token,
                Expires = expires,
                User = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role
                }
            };
        }
    }

}
