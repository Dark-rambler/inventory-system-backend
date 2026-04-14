using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Application.Services.InventoryMovementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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
            return Ok(await service.CreateInventoryMovementAsync(request));
        }
    }
}
