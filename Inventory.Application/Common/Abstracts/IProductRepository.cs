using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IProductRepository
    {
        public Task<Product?> GetProductByIdAsync(Guid id);
    }
}
