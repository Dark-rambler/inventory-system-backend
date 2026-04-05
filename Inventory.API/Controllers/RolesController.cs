using Inventory.Application.DataTransferObjects.RoleDto;
using Inventory.Application.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController(IRoleService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRolesAsync()
        {
            return Ok(await service.GetRolesAsync());
        }
    }
}