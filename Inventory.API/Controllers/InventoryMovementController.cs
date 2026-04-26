using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Application.Services.InventoryMovementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryMovementController(IInventoryMovementService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetInventoryMovementsAsync([FromQuery] InventoryMovementSearchParams searchParams)
        {
            return Ok(await service.GetInventoryMovementsAsync(searchParams));
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryMovementAsync(InventoryMovementRequest request)
        {
            Guid user = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await service.CreateInventoryMovementAsync(request, user));
        }
    }
}
