using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface ICategoryRepository
    {
        Task<PaginatedList<Category>> GetCategoriesAsync(Guid businessId, string? name, int page, int pageSize);
        Task<Category?> GetCategoryByIdAsync(int id, Guid businessId);
        Task<Category> CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
