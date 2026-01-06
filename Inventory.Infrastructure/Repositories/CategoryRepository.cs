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
        public async Task<PaginatedList<Category>> GetCategoriesAsync(string? name, int page, int pageSize)
        {
            var query = context.Categories
                .Include(c => c.Products)
                .AsQueryable();
            return await query
                .FiltersCategory(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return await context.Categories
                .FirstAsync(c => c.Id == category.Id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
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
