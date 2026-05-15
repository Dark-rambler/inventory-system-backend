using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Application.Services.WarehouseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehouseController(IWarehouseService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<WarehouseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehousesAsync([FromQuery] WarehouseSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetWarehousesAsync(searchParams, businessId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehouseByIdAsync(Guid id, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetWarehouseByIdAsync(id, businessId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateWarehouseAsync([FromBody] WarehouseRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateWarehouseAsync(request, businessId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateWarehouseAsync(Guid id, [FromBody] WarehouseRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.UpdateWarehouseAsync(id, request, businessId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteWarehouseAsync(Guid id, [FromHeader][BindRequired] Guid businessId)
        {
            await service.DeleteWarehouseAsync(id, businessId);
            return NoContent();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(PaginatedList<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWarehousesByProductAndQuantityAsync(Guid id, [FromQuery] ProductSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetProductsByWarehousesAsync(id, searchParams, businessId));
        }

        [HttpPost("{id}/products")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddProductsToWarehouseAsync(Guid id, [FromBody] IEnumerable<WarehouseProductRequest> request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.AddProductsToWarehouseAsync(id, request, businessId);
            return NoContent();
        }

        [HttpGet("{id}/products/doesnt-exist")]
        public async Task<IActionResult> GetProductsDoesntExistByWarehouseAsync(Guid id, [FromQuery] ProductSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetProductsDoesntExistByWarehouseAsync(id, searchParams, businessId));
        }
    }
}