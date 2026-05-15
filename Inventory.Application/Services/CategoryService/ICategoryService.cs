using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CategoryDto;

namespace Inventory.Application.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<PaginatedList<CategoryResponse>> GetCategoriesAsync(CategorySearchParams searchParams, Guid businessId);
        Task<CategoryResponse> GetCategoryByIdAsync(int id, Guid businessId);
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request, Guid businessId);
        Task UpdateCategoryAsync(int id, CategoryRequest request, Guid businessId);
        Task DeleteCategoryAsync(int id, Guid businessId);
    }
}
