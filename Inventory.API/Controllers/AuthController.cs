using Inventory.Application.DataTransferObjects.AuthDto;
using Inventory.Application.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            return Ok(await service.LoginAsync(request));
        }
    }
}
