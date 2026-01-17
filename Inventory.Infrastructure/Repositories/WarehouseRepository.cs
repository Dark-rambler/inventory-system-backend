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
        public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        {
            context.Warehouses.Add(warehouse);
            await context.SaveChangesAsync();
            return await context.Warehouses
                .FirstAsync(w => w.Id == warehouse.Id);
        }

        public async Task DeleteWarehouseAsync(Warehouse warehouse)
        {
            warehouse.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<Warehouse?> GetWarehouseByIdAsync(Guid id)
        {
            return await context.Warehouses
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<PaginatedList<Warehouse>> GetWarehousesAsync(string? name, int page, int pageSize)
        {
            var query = context.Warehouses
                .AsQueryable();
            return await query
                .OrderByDescending(w => w.CreatedAt)
                .FiltersWarehouse(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            context.Warehouses.Update(warehouse);
            await context.SaveChangesAsync();
        }
    }
}
