using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<PaginatedList<CategoryResponse>> GetCategoriesAsync(CategorySearchParams searchParams);
        Task<CategoryResponse> GetCategoryByIdAsync(int id);
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);
        Task UpdateCategoryAsync(int id, CategoryRequest request);
        Task DeleteCategoryAsync(int id);
    }
}
