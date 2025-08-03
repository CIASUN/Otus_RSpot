using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RSpot.Booking.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Получение тестового JWT токена (для тестирования)
        /// </summary>
        /// <returns>JWT токен</returns>
        [HttpGet("token")]
        [SwaggerOperation(
            Summary = "Получить тестовый JWT токен",
            Description = "Генерирует JWT токен для тестового пользователя (без реальной проверки)"
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetToken()
        {
            // В реальном приложении — здесь будет проверка пользователя и пароля
            var token = _tokenGenerator.GenerateToken("123", "TestUser");
            return Ok(new { token });
        }

        /// <summary>
        /// Защищённый эндпоинт, доступный только с валидным JWT
        /// </summary>
        /// <returns>Приветственное сообщение для аутентифицированного пользователя</returns>
        [Authorize]
        [HttpGet("secure")]
        [SwaggerOperation(
            Summary = "Защищённый эндпоинт",
            Description = "Возвращает приветственное сообщение для аутентифицированного пользователя"
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Secure()
        {
            var userName = User.Identity?.Name ?? "anonymous";
            return Ok(new { message = $"Привет, {userName}! Вы успешно аутентифицированы." });
        }
    }
}
