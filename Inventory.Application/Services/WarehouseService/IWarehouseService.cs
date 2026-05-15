using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;

namespace Inventory.Application.Services.WarehouseService
{
    public interface IWarehouseService
    {
        Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request, Guid businessId);
        Task<WarehouseResponse> GetWarehouseByIdAsync(Guid id, Guid businessId);
        Task<PaginatedList<WarehouseResponse>> GetWarehousesAsync(WarehouseSearchParams searchParams, Guid businessId);
        Task UpdateWarehouseAsync(Guid id, WarehouseRequest request, Guid businessId);
        Task DeleteWarehouseAsync(Guid id, Guid businessId);
        Task<PaginatedList<WarehouseProductResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
        Task AddProductsToWarehouseAsync(Guid id, IEnumerable<WarehouseProductRequest> request, Guid businessId);
        Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByWarehouseAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
    }
}
