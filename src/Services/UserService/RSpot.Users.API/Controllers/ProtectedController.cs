using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RSpot.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("ping")]
        [Authorize]
        public IActionResult Ping() => Ok("🟢 You are authorized!");
    }

}
