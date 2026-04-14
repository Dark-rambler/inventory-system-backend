using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IInventoryMovementRepository
    {
        Task<InventoryMovement> CreateInventoryMovementAsync(InventoryMovement inventoryMovement, WarehouseProduct? warehouseProduct, BranchProduct? branch);
        Task<PaginatedList<InventoryMovement>> GetInventoryMovementsAsync(int page, int pageSize);
    }
}
