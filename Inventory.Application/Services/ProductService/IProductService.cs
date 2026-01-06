using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Services.ProductService
{
    public interface IProductService
    {
        Task<PaginatedList<ProductResponse>> GetProductsAsync(ProductSearchParams searchParams);
        Task<ProductResponse> GetProductByIdAsync(Guid id);
        Task<ProductResponse> CreateProductAsync(ProductRequest request);
        Task UpdateProductAsync(Guid id, ProductRequest request);
        Task DeleteProductAsync(Guid id);
    }
}
