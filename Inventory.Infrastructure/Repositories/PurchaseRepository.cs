using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class PurchaseRepository(InventoryDbContext context) : IPurchaseRepository
    {
        public async Task CreatePurchaseAsync(Purchase purchase, List<InventoryMovement> inventoryMovements, List<BranchProduct>? productsByBranchUpdated, List<WarehouseProduct>? productsByWarehouseUpdated, AuditHistory auditHistory)
        {
            context.Purchases.Add(purchase);
            context.InventoryMovements.AddRange(inventoryMovements);
            if (productsByBranchUpdated != null)
            {
                context.BranchProducts.UpdateRange(productsByBranchUpdated);
            }
            if (productsByWarehouseUpdated != null)
            {
                context.WarehouseProducts.UpdateRange(productsByWarehouseUpdated);
            }
            context.AuditHistories.Add(auditHistory);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Purchase>> GetPurchasesAsync(Guid businessId, DateTime? fromDate, DateTime? toDate, Guid? providerId, Guid? branchId, int page, int pageSize) =>
            await context.Purchases
                .AsQueryable()
                .Where(p => p.BusinessId == businessId)
                .Include(p => p.Provider)
                .Include(p => p.Buyer)
                .Include(p => p.PurchaseDetails)
                    .ThenInclude(pd => pd.Product)
                .FiltersPurchases(fromDate, toDate, providerId)
                .OrderByDescending(b => b.Date)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds) =>
            await context.BranchProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BranchId == branchId && productIds.Contains(bp.ProductId))
                .ToListAsync();

        public async Task<IEnumerable<WarehouseProduct>> GetWarehouseProductsByProductIdsAsync(Guid warehouseId, IEnumerable<int> productIds) =>
            await context.WarehouseProducts
                .Include(wp => wp.Product)
                .Where(wp => wp.WarehouseId == warehouseId && productIds.Contains(wp.ProductId))
                .ToListAsync();
    }
}