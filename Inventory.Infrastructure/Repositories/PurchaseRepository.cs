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
        public async Task CreatePurchaseAsync(Purchase purchase, List<InventoryMovement> inventoryMovements, List<BranchProduct> productsUpdated, AuditHistory auditHistory)
        {
            context.Purchases.Add(purchase);
            context.InventoryMovements.AddRange(inventoryMovements);
            context.BranchProducts.UpdateRange(productsUpdated);
            context.AuditHistories.Add(auditHistory);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Purchase>> GetPurchasesAsync(DateTime? fromDate, DateTime? toDate, Guid? providerId, Guid? branchId, int page, int pageSize)
        {
            var query = context.Purchases
                .AsQueryable();
            return await query
                .Include(p => p.Provider)
                .Include(p => p.Branch)
                .Include(p => p.Buyer)
                .Include(p => p.PurchaseDetails)
                    .ThenInclude(pd => pd.Product)
                .FiltersPurchases(fromDate, toDate, providerId, branchId)
                .OrderByDescending(b => b.Date)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds)
        {
            return await context.BranchProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BranchId == branchId && productIds.Contains(bp.ProductId))
                .ToListAsync();
        }
    }
}