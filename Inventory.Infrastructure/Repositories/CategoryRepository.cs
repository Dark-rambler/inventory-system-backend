using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class CategoryRepository(InventoryDbContext context) : ICategoryRepository
    {
        public async Task<PaginatedList<Category>> GetCategoriesAsync(Guid businessId, string? name, int page, int pageSize)
        {
            var query = context.Categories
                .AsQueryable();
            return await query
                .Where(c => c.BusinessId == businessId)
                .OrderByDescending(b => b.CreatedAt)
                .FiltersCategory(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Category?> GetCategoryByIdAsync(int id) =>
            await context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return await context.Categories
                .FirstAsync(c => c.Id == category.Id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            category.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}
