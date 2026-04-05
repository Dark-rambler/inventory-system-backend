using Inventory.Application.DataTransferObjects;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.BranchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BranchController(IBranchService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBranchesAsync([FromQuery] BranchSearchParams searchParams)
        {
            return Ok(await service.GetBranchesAsync(searchParams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBranchByIdAsync(Guid id)
        {
            return Ok(await service.GetBranchByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBranchAsync([FromBody] BranchRequest request)
        {
            var result = await service.CreateBranchAsync(request);
            return CreatedAtAction(nameof(GetBranchByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateBranchAsync(Guid id, [FromBody] BranchRequest request)
        {
            await service.UpdateBranchAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteBranchAsync(Guid id)
        {
            await service.DeleteBranchAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByBranchAsync(Guid id, [FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsByBranchAsync(id, searchParams));
        }

        [HttpPut("{id}/products/add-stock")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddStockAsync(Guid id, [FromBody] AddStockRequest request)
        {
            await service.AddStockAsync(id, request);
            return NoContent();
        }
    }
}