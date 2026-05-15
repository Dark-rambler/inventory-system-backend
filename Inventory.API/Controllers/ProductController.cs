using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetProductsAsync(searchParams, businessId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductByIdAsync(int id, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetProductByIdAsync(id, businessId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateProductAsync(request, businessId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.UpdateProductAsync(id, request, businessId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProductAsync(int id, [FromHeader][BindRequired] Guid businessId)
        {
            await service.DeleteProductAsync(id, businessId);
            return NoContent();
        }

        [HttpPost("bulk-upload")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkUploadProductsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only .xlsx files are allowed");

            using var stream = file.OpenReadStream();
            await service.BulkUploadProductsAsync(stream);
            return Ok();
        }

        [HttpGet("template")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBulkUploadTemplate()
        {
            var stream = service.GetBulkUploadTemplate();
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products_template.xlsx");
        }
    }
}