using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] CategorySearchParams searchParams)
        {
            return Ok(await service.GetCategoriesAsync(searchParams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid id)
        {
            return Ok(await service.GetCategoryByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequest request)
        {
            var result = await service.CreateCategoryAsync(request);
            return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCategoryAsync(Guid id, [FromBody] CategoryRequest request)
        {
            await service.UpdateCategoryAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCategoryAsync(Guid id)
        {
            await service.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}