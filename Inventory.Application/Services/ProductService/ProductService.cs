using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.ProductService
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await repository.GetProductByIdAsync(id) ?? throw new KeyNotFoundException("Not found");
        }
    }
}
