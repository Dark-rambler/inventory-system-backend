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
                .AsQueryable();
            return await query
                .FiltersProduct(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            context.Products.Add(product);
            return context.SaveChangesAsync().ContinueWith(_ => product);
        }

        public Task UpdateProductAsync(Product product)
        {
            context.Products.Update(product);
            return context.SaveChangesAsync();
        }
    }
}
