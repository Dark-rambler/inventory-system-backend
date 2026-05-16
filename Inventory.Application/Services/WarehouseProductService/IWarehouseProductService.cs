using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;

namespace Inventory.Application.Services.WarehouseProductService
{
    public interface IWarehouseProductService
    {
        Task<PaginatedList<WarehouseProductResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
        Task AddProductsToWarehouseAsync(Guid id, IEnumerable<WarehouseProductRequest> request, Guid businessId);
        Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByWarehouseAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
    }
}
