using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.WarehouseDto;

namespace Inventory.Application.Services.WarehouseService
{
    public interface IWarehouseService
    {
        Task<WarehouseResponse> CreateWarehouseAsync(WarehouseRequest request, Guid businessId);
        Task<WarehouseResponse> GetWarehouseByIdAsync(Guid id, Guid businessId);
        Task<PaginatedList<WarehouseResponse>> GetWarehousesAsync(WarehouseSearchParams searchParams, Guid businessId);
        Task UpdateWarehouseAsync(Guid id, WarehouseRequest request, Guid businessId);
        Task DeleteWarehouseAsync(Guid id, Guid businessId);
    }
}
