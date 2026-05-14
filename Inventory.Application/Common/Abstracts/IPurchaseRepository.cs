using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IPurchaseRepository
    {
        Task CreatePurchaseAsync(Purchase purchase, List<InventoryMovement> inventoryMovements, List<BranchProduct>? productsByBranchUpdated, List<WarehouseProduct>? productsByWarehouseUpdated, AuditHistory auditHistory);
        Task<PaginatedList<Purchase>> GetPurchasesAsync(Guid businessId, DateTime? fromDate, DateTime? toDate, Guid? providerId, Guid? branchId, int page, int pageSize);
        Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds);
        Task<IEnumerable<WarehouseProduct>> GetWarehouseProductsByProductIdsAsync(Guid warehouseId, IEnumerable<int> productIds);
    }
}