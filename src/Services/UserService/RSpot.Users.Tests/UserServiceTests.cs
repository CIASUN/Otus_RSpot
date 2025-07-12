using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Identity;
using RSpot.Users.Application.Services;
using RSpot.Users.Application.DTOs;
using RSpot.Users.Domain.Interfaces;
using RSpot.Users.Domain.Models;

namespace RSpot.Users.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<IJwtTokenGenerator> _tokenGenMock;
        private readonly IPasswordHasher<User> _hasher;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _tokenGenMock = new Mock<IJwtTokenGenerator>();
            _hasher = new PasswordHasher<User>();

            _service = new UserService(
                _repoMock.Object,
                _tokenGenMock.Object,
                _hasher
            );
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnAuthResponse_WhenUserIsNew()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                FullName = "Test User",
                Password = "Password123"
            };

            _repoMock.Setup(r => r.ExistsByEmailAsync(request.Email)).ReturnsAsync(false);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _tokenGenMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("mock-token");
            _tokenGenMock.Setup(t => t.GetExpiration(It.IsAny<string>())).Returns(DateTime.UtcNow.AddHours(1));

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("mock-token", result.Token);
            Assert.Equal("test@example.com", result.User.Email);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnNull_WhenEmailAlreadyExists()
        {
            _repoMock.Setup(r => r.ExistsByEmailAsync("duplicate@example.com")).ReturnsAsync(true);

            var result = await _service.RegisterAsync(new RegisterRequest
            {
                Email = "duplicate@example.com",
                FullName = "Duplicate",
                Password = "pass"
            });

            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnAuthResponse_WhenCredentialsAreValid()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Name = "Test User"
            };
            user.PasswordHash = _hasher.HashPassword(user, "Password123");

            _repoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);
            _tokenGenMock.Setup(t => t.GenerateToken(user)).Returns("valid-token");
            _tokenGenMock.Setup(t => t.GetExpiration("valid-token")).Returns(DateTime.UtcNow.AddMinutes(60));

            var result = await _service.LoginAsync(new LoginRequest
            {
                Email = user.Email,
                Password = "Password123"
            });

            Assert.NotNull(result);
            Assert.Equal("valid-token", result.Token);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            var user = new User
            {
                Email = "fail@example.com",
                PasswordHash = _hasher.HashPassword(new User(), "correctpass")
            };

            _repoMock.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(user);

            var result = await _service.LoginAsync(new LoginRequest
            {
                Email = user.Email,
                Password = "wrongpass"
            });

            Assert.Null(result);
        }
    }
}
