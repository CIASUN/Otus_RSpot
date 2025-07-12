using Microsoft.AspNetCore.Mvc;
using RSpot.Users.Application.Services.Interfaces;
using RSpot.Users.Application.Services;
using RSpot.Users.Application.Configuration;
using RSpot.Users.Application.DTOs;

namespace RSpot.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;

        public AuthController(IJwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 🟡 Заглушка. В реальности- по базе
            if (request.Email == "admin@example.com" && request.Password == "admin")
            {
                var token = _jwtService.GenerateToken("admin-id", "Admin");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }

    //public class LoginRequest
    //{
    //    public string Email { get; set; } = string.Empty;
    //    public string Password { get; set; } = string.Empty;
    //}

}
