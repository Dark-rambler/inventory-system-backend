using Inventory.Domain.Entities;

namespace Inventory.Application.Services.ProductService
{
    public interface IProductService
    {
        public Task<Product> GetProductByIdAsync(Guid id);
    }
}
