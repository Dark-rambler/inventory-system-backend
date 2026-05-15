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
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            return Ok(await service.LoginAsync(request));
        }
    }
}
