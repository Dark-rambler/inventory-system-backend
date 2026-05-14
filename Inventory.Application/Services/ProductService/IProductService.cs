using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Services.ProductService
{
    public interface IProductService
    {
        Task<PaginatedList<ProductResponse>> GetProductsAsync(ProductSearchParams searchParams, Guid businessId);
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<ProductResponse> CreateProductAsync(ProductRequest request, Guid businessId);
        Task UpdateProductAsync(int id, ProductRequest request);
        Task DeleteProductAsync(int id);
        Task BulkUploadProductsAsync(Stream fileStream);
        Stream GetBulkUploadTemplate();
    }
}
