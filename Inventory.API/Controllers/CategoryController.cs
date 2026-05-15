using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] CategorySearchParams searchParams, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetCategoriesAsync(searchParams, businessId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.GetCategoryByIdAsync(id, businessId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            return Ok(await service.CreateCategoryAsync(request, businessId));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] CategoryRequest request, [FromHeader][BindRequired] Guid businessId)
        {
            await service.UpdateCategoryAsync(id, request, businessId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCategoryAsync(int id, [FromHeader][BindRequired] Guid businessId)
        {
            await service.DeleteCategoryAsync(id, businessId);
            return NoContent();
        }
    }
}