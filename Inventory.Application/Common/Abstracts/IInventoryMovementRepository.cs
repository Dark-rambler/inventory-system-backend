using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Common.Abstracts
{
    public interface IInventoryMovementRepository
    {
        Task<InventoryMovement> CreateInventoryMovementAsync(InventoryMovement inventoryMovement, WarehouseProduct? warehouseProduct, BranchProduct? branch, AuditHistory auditHistory);
        Task<PaginatedList<InventoryMovement>> GetInventoryMovementsAsync(Guid businessId, Guid? warehouseId, Guid? branchId, EnumMovementType? movementType, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
    }
}
