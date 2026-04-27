using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IPurchaseRepository
    {
        Task CreatePurchaseAsync(Purchase purchase, List<InventoryMovement> inventoryMovements, List<BranchProduct> productsUpdated, AuditHistory auditHistory);
        Task<PaginatedList<Purchase>> GetPurchasesAsync(DateTime? fromDate, DateTime? toDate, Guid? providerId, Guid? branchId, int page, int pageSize);
        Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds);
    }
}