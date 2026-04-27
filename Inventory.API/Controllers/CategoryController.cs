using Inventory.Application.Common.Pagination;
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
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedList<CategoryResponse>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequest request)
        {
            return Ok(await service.CreateCategoryAsync(request));
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