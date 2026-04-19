using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository(InventoryDbContext context) : IProductRepository
    {
        public async Task<PaginatedList<Product>> GetProductsAsync(string? name, int page, int pageSize)
        {
            var query = context.Products
                .Include(c => c.Category)
                .Include(c => c.Measure)
                .AsQueryable();
            return await query
                .OrderByDescending(b => b.CreatedAt)
                .FiltersProduct(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await context.Products
                .Include(p => p.Category)
                .Include(c => c.Measure)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return await context.Products
                .Include(p => p.Category)
                .Include(c => c.Measure)
                .FirstAsync(p => p.Id == product.Id);
        }


        public async Task UpdateProductAsync(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            product.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}
