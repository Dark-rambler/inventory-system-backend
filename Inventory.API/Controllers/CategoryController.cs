using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] string? name, [FromQuery] int page = 1, int pageSize = 10)
        {
            return Ok(await service.GetCategoriesAsync(name, page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(Guid id)
        {
            return Ok(await service.GetCategoryByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequest request)
        {
            return Ok(await service.CreateCategoryAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(Guid id, [FromBody] CategoryRequest request)
        {
            await service.UpdateCategoryAsync(id, request);
            return NoContent();
        }
    }
}
