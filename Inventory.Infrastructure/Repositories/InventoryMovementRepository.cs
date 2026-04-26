using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryMovementRepository(InventoryDbContext context) : IInventoryMovementRepository
    {
        public async Task<InventoryMovement> CreateInventoryMovementAsync(InventoryMovement inventoryMovement, WarehouseProduct? warehouseProduct, BranchProduct? branch, AuditHistory auditHistory)
        {
            if (warehouseProduct != null)
            {
                context.WarehouseProducts.Update(warehouseProduct);
            }
            if (branch != null)
            {
                context.BranchProducts.Update(branch);
            }
            context.InventoryMovements.Add(inventoryMovement);
            context.AuditHistories.Add(auditHistory);
            await context.SaveChangesAsync();
            return await context.InventoryMovements
                .Include(im => im.Product)
                .Include(im => im.FromWarehouse)
                .Include(im => im.ToWarehouse)
                .Include(im => im.FromBranch)
                .Include(im => im.ToBranch)
                .FirstAsync(im => im.Id == inventoryMovement.Id);
        }

        public async Task<PaginatedList<InventoryMovement>> GetInventoryMovementsAsync(Guid? warehouseId, Guid? branchId, EnumMovementType? movementType, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = context.InventoryMovements
                .AsQueryable();
            return await query
                .Include(im => im.Product)
                .Include(im => im.FromWarehouse)
                .Include(im => im.ToWarehouse)
                .Include(im => im.FromBranch)
                .Include(im => im.ToBranch)
                .FiltersInventoryMovement(warehouseId, branchId, movementType, fromDate, toDate)
                .OrderByDescending(b => b.CreatedAt)
                .ToPaginatedListAsync(page, pageSize);
        }
    }
}
