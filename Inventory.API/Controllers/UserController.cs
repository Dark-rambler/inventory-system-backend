using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Application.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserSearchParams searchParams)
        {
            return Ok(await service.GetUsersAsync(searchParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            return Ok(await service.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request)
        {
            return Ok(await service.CreateUserAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserRequest request)
        {
            await service.UpdateUserAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await service.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
