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

        public async Task AddStockAsync(Guid warehouseId, Guid productId, int stock)
        {
            var warehouseProduct = await context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId);

            if (warehouseProduct == null)
            {
                warehouseProduct = new WarehouseProduct
                {
                    WarehouseId = warehouseId,
                    ProductId = productId,
                    Stock = stock
                };
                context.WarehouseProducts.Add(warehouseProduct);
            }
            else
            {
                warehouseProduct.Stock += stock;
            }

            await context.SaveChangesAsync();
        }

        public async Task ReduceStockAsync(Guid warehouseId, Guid productId, int stock)
        {
            var warehouseProduct = await context.WarehouseProducts
                .FirstOrDefaultAsync(wp => wp.WarehouseId == warehouseId && wp.ProductId == productId) ?? throw new KeyNotFoundException($"Product with id {productId} not found in warehouse {warehouseId}");
            if (warehouseProduct.Stock < stock)
            {
                throw new InvalidOperationException($"Insufficient stock in warehouse. Available: {warehouseProduct.Stock}, requested: {stock}");
            }

            warehouseProduct.Stock -= stock;
            await context.SaveChangesAsync();
        }
    }
}
