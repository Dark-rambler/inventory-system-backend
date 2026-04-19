using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class WarehouseRepository(InventoryDbContext context) : IWarehouseRepository
    {
        public async Task<PaginatedList<Warehouse>> GetWarehousesAsync(string? name, int page, int pageSize)
        {
            var query = context.Warehouses
                .AsQueryable();
            return await query
                .Include(w => w.Location)
                .OrderByDescending(w => w.CreatedAt)
                .FiltersWarehouse(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(Guid id)
        {
            return await context.Warehouses
                .Include(w => w.Location)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        {
            context.Locations.Add(warehouse.Location);
            context.Warehouses.Add(warehouse);
            await context.SaveChangesAsync();
            return await context.Warehouses
                .Include(w => w.Location)
                .FirstAsync(w => w.Id == warehouse.Id);
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            warehouse.UpdatedAt = DateTime.UtcNow;
            context.Warehouses.Update(warehouse);
            await context.SaveChangesAsync();
        }

        public async Task DeleteWarehouseAsync(Warehouse warehouse)
        {
            warehouse.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<WarehouseProduct>> GetProductsByWarehousesAsync(Guid id, string? name, int page, int pageSize)
        {
            var query = context.WarehouseProducts
                .AsQueryable();
            return await query
                .Where(wp => wp.WarehouseId == id)
                .Include(wp => wp.Product)
                .ThenInclude(wp => wp.Category)
                .OrderByDescending(wp => wp.Product.CreatedAt)
                .FiltersWarehouseProduct(name)
                .ToPaginatedListAsync(page, pageSize);
        }
        public async Task<WarehouseProduct?> GetWarehouseProductByWarehouseIdAndProductIdAsync(Guid? warehouseId, Guid productId)
        {
            return await context.WarehouseProducts
                .Include(wp => wp.Product)
                .FirstOrDefaultAsync(bp => warehouseId.HasValue && bp.WarehouseId == warehouseId.Value && bp.ProductId == productId);
        }
    }
}
