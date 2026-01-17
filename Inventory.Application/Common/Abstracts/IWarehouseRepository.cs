using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IWarehouseRepository
    {
        Task<Warehouse> CreateWarehouseAsync(Warehouse branch);
        Task<Warehouse?> GetWarehouseByIdAsync(Guid id);
        Task<PaginatedList<Warehouse>> GetWarehousesAsync(string? name, int page, int pageSize);
        Task UpdateWarehouseAsync(Warehouse branch);
        Task DeleteWarehouseAsync(Warehouse branch);
    }
}
