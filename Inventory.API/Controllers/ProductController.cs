using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchParams searchParams)
        {
            return Ok(await service.GetProductsAsync(searchParams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductByIdAsync(Guid id)
        {
            return Ok(await service.GetProductByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductRequest request)
        {
            return Ok(await service.CreateProductAsync(request));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProductAsync(Guid id, [FromBody] ProductRequest request)
        {
            await service.UpdateProductAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            await service.DeleteProductAsync(id);
            return NoContent();
        }
    }
}