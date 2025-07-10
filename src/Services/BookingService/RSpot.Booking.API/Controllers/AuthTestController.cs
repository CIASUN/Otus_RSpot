using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSpot.Booking.Application.Interfaces;

namespace RSpot.Booking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTestController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthTestController(IJwtTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        // Получение тестового JWT токена (для тестирования)
        [HttpGet("token")]
        public IActionResult GetToken()
        {
            // В реальном приложении — здесь будет проверка пользователя и пароля
            var token = _tokenGenerator.GenerateToken("123", "TestUser");
            return Ok(new { token });
        }

        // Защищённый эндпоинт, доступен только с валидным JWT
        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            var userName = User.Identity?.Name ?? "anonymous";
            return Ok(new { message = $"Привет, {userName}! Вы успешно аутентифицированы." });
        }
    }
}
