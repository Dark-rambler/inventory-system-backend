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
    //[Authorize]
    public class BranchController(IBranchService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetBranchesAsync([FromQuery] BranchSearchParams searchParams)
        {
            return Ok(await service.GetBranchesAsync(searchParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchByIdAsync(Guid id)
        {
            return Ok(await service.GetBranchByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranchAsync([FromBody] BranchRequest request)
        {
            return Ok(await service.CreateBranchAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranchAsync(Guid id, [FromBody] BranchRequest request)
        {
            await service.UpdateBranchAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranchAsync(Guid id)
        {
            await service.DeleteBranchAsync(id);
            return NoContent();
        }


        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetProductsByBranchAsync(Guid id, [FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsByBranchAsync(id, searchParams));
        }

        [HttpPut("{id}/products/add-stock")]
        public async Task<IActionResult> AddStockAsync(Guid id, [FromBody] AddStockRequest request)
        {
            await service.AddStockAsync(id, request);
            return NoContent();
        }
    }
}
