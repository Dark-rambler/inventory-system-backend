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
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Category> CreateCategoryAsync(Category category)
        {
            context.Categories.Add(category);
            return context.SaveChangesAsync().ContinueWith(_ => category);
        }

        public Task UpdateCategoryAsync(Category category)
        {
            context.Categories.Update(category);
            return context.SaveChangesAsync();
        }
    }
}
