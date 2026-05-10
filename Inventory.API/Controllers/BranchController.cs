using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.BranchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BranchController(IBranchService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<BranchResponse>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(BranchResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBranchAsync([FromBody] BranchRequest request)
        {
            return Ok(await service.CreateBranchAsync(request));
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
        [ProducesResponseType(typeof(PaginatedList<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByBranchAsync(Guid id, [FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsByBranchAsync(id, searchParams));
        }

        [HttpPost("{id}/products")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddProductToBranchAsync(Guid id, [FromBody] IEnumerable<BranchProductRequest> request)
        {
            await service.AddProductsToBranchAsync(id, request);
            return NoContent();
        }

        [HttpPost("{id}/sales")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateSaleAsync(Guid id, [FromBody] SaleRequest request)
        {
            Guid user = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value); //temporal
            await service.CreateSaleAsync(id, request, user);
            return NoContent();
        }

        [HttpGet("{id}/sales")]
        [ProducesResponseType(typeof(PaginatedList<SaleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSalesByBranchAsync(Guid id, [FromQuery] SaleSearchParams searchParams)
        {
            return Ok(await service.GetSalesByBranchAsync(id, searchParams));
        }

        [HttpGet("{id}/products/doesnt-exist")]
        public async Task<IActionResult> GetProductsDoesntExistByBranchAsync(Guid id, [FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsDoesntExistByBranchAsync(id, searchParams));
        }
    }
}