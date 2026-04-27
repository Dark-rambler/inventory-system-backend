using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IProductRepository
    {
        Task<PaginatedList<Product>> GetProductsAsync(string? name, int page, int pageSize);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<List<Product>> BulkCreateAsync(List<Product> products);
    }
}
