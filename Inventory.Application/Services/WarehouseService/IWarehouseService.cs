using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;

namespace Inventory.Application.Services.WarehouseService
{
    public interface IWarehouseService
    {
        Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request);
        Task<WarehouseResponse> GetWarehouseByIdAsync(Guid id);
        Task<PaginatedList<WarehouseResponse>> GetWarehousesAsync(WarehouseSearchParams searchParams);
        Task UpdateWarehouseAsync(Guid id, WarehouseRequest request);
        Task DeleteWarehouseAsync(Guid id);
        Task<PaginatedList<WarehouseResponse>> GetProductsByWarehousesAsync(Guid id, ProductSearchParams searchParams);
    }
}
