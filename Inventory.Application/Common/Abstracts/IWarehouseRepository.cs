using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IWarehouseRepository
    {
        Task<PaginatedList<Warehouse>> GetWarehousesAsync(Guid businessId, string? name, int page, int pageSize);
        Task<Warehouse?> GetWarehouseByIdAsync(Guid id, Guid businessId);
        Task<Warehouse> CreateWarehouseAsync(Warehouse branch);
        Task UpdateWarehouseAsync(Warehouse branch);
        Task DeleteWarehouseAsync(Warehouse branch);
        Task<PaginatedList<WarehouseProduct>> GetProductsByWarehousesAsync(Guid id, string? name, int page, int pageSize);
        Task<WarehouseProduct?> GetWarehouseProductByWarehouseIdAndProductIdAsync(Guid? warehouseId, int productId);
        Task AddProductsToWarehouseAsync(List<WarehouseProduct> warehouseProducts);
        Task<PaginatedList<Product>> GetProductsDoesntExistByWarehouseAsync(Guid id, Guid businessId, int page, int pageSize);
    }
}
