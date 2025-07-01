using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RSpot.Users.Application.Services.Interfaces;
using RSpot.Users.Application.DTOs;

namespace RSpot.Users.API.Controllers
{

    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);
            return result is null ? BadRequest("Email already exists.") : Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginAsync(request);
            return result is null ? Unauthorized() : Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.GetCurrentUserAsync(Guid.Parse(userId!));
            return result is null ? NotFound() : Ok(result);
        }
    }
}
