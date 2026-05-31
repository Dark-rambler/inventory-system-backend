using Inventory.Application.Common.Abstracts;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class DashboardRepository(InventoryDbContext context) : IDashboardRepository
    {
        public async Task<int> GetTodaySalesCountAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end)
        {
            var query = context.Sales.Where(s => s.BusinessId == businessId && s.Date >= start && s.Date < end);
            if (branchId.HasValue)
                query = query.Where(s => s.BranchId == branchId.Value);
            return await query.CountAsync();
        }

        public async Task<double> GetTodaySalesTotalAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end)
        {
            var query = context.Sales.Where(s => s.BusinessId == businessId && s.Date >= start && s.Date < end);
            if (branchId.HasValue)
                query = query.Where(s => s.BranchId == branchId.Value);
            return await query.SumAsync(s => (double?)s.Total) ?? 0;
        }

        public async Task<int> GetTodayMovementsCountAsync(Guid businessId, Guid? branchId, DateTime start, DateTime end)
        {
            var query = context.InventoryMovements
                .Where(im => im.BusinessId == businessId && im.CreatedAt >= start && im.CreatedAt < end);
            if (branchId.HasValue)
                query = query.Where(im => im.FromBranchId == branchId.Value || im.ToBranchId == branchId.Value);
            return await query.CountAsync();
        }

        public async Task<int> GetLowStockProductsCountAsync(Guid businessId, Guid? branchId)
        {
            if (branchId.HasValue)
                return await context.BranchProducts
                    .CountAsync(bp => bp.BranchId == branchId.Value && bp.Stock <= bp.LowStock);

            var branchLow = context.BranchProducts
                .Where(bp => bp.Branch.BusinessId == businessId && bp.Stock <= bp.LowStock)
                .Select(bp => bp.ProductId);
            var warehouseLow = context.WarehouseProducts
                .Where(wp => wp.Warehouse.BusinessId == businessId && wp.Stock <= wp.LowStock)
                .Select(wp => wp.ProductId);
            return await branchLow.Union(warehouseLow).CountAsync();
        }
    }
}
