using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<PaginatedList<CategoryResponse>> GetCategoriesAsync(string? name, int page, int pageSize);
        Task<CategoryResponse> GetCategoryByIdAsync(Guid id);
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);
        Task UpdateCategoryAsync(Guid id, CategoryRequest request);
    }
}
