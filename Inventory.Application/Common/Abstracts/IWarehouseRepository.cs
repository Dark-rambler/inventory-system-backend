using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IWarehouseRepository
    {
        Task<PaginatedList<Warehouse>> GetWarehousesAsync(string? name, int page, int pageSize);
        Task<Warehouse?> GetWarehouseByIdAsync(Guid id);
        Task<Warehouse> CreateWarehouseAsync(Warehouse branch);
        Task UpdateWarehouseAsync(Warehouse branch);
        Task DeleteWarehouseAsync(Warehouse branch);
        Task<PaginatedList<WarehouseProduct>> GetProductsByWarehousesAsync(Guid id, string? name, int page, int pageSize);
        Task<WarehouseProduct?> GetWarehouseProductByWarehouseIdAndProductIdAsync(Guid? warehouseId, Guid productId);
    }
}
