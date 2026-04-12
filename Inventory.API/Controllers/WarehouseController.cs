using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.Services.WarehouseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehouseController(IWarehouseService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehousesAsync([FromQuery] WarehouseSearchParams searchParams)
        {
            return Ok(await service.GetWarehousesAsync(searchParams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehouseByIdAsync(Guid id)
        {
            return Ok(await service.GetWarehouseByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarehouseAsync([FromBody] WarehouseRequest request)
        {
            return Ok(await service.CreateWarehouseAsync(request));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateWarehouseAsync(Guid id, [FromBody] WarehouseRequest request)
        {
            await service.UpdateWarehouseAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWarehouseAsync(Guid id)
        {
            await service.DeleteWarehouseAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehousesByProductAndQuantityAsync(Guid id, [FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsByWarehousesAsync(id, searchParams));
        }
    }
}