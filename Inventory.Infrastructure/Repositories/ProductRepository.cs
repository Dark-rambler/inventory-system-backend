using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class ProductRepository(InventoryDbContext context) : IProductRepository
    {
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
