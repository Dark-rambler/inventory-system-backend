using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetUsersAsync(searchParams, businessId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByIdAsync(Guid id, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetUserByIdAsync(id, businessId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateUserAsync(request, businessId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.UpdateUserAsync(id, request, businessId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUserAsync(Guid id, [FromHeader][BindRequired] Guid businessId)
        {
            await service.DeleteUserAsync(id, businessId);
            return NoContent();
        }
    }
}