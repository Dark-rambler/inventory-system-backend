using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class BranchRepository(InventoryDbContext context) : IBranchRepository
    {
        public async Task<PaginatedList<Branch>> GetBranchesAsync(string? name, int page, int pageSize)
        {
            var query = context.Branches
                .AsQueryable();
            return await query
                .Include(b => b.Location)
                .OrderByDescending(b => b.CreatedAt)
                .FiltersBranch(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Branch?> GetBranchByIdAsync(Guid id)
        {
            return await context.Branches
                .Include(w => w.Location)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            context.Branches.Add(branch);
            await context.SaveChangesAsync();
            return await context.Branches
                .Include(w => w.Location)
                .FirstAsync(b => b.Id == branch.Id);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            branch.UpdatedAt = DateTime.UtcNow;
            context.Branches.Update(branch);
            await context.SaveChangesAsync();
        }

        public async Task DeleteBranchAsync(Branch branch)
        {
            branch.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<BranchProduct>> GetProductsByBranchAsync(Guid id, string? name, int page, int pageSize)
        {
            var query = context.BranchProducts
                .AsQueryable();
            return await query
                .Where(bp => bp.BranchId == id)
                .Include(bp => bp.Product)
                .ThenInclude(bp => bp.Category)
                .OrderByDescending(bp => bp.Product.CreatedAt)
                .FiltersBranchProduct(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<int> productIds)
        {
            return await context.BranchProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BranchId == branchId && productIds.Contains(bp.ProductId))
                .ToListAsync();
        }

        public async Task CreateSaleAsync(Sale sale, List<InventoryMovement> intentoryMovements, List<BranchProduct> productsUpdated, AuditHistory auditHistory)
        {
            context.Sales.Add(sale);
            context.InventoryMovements.AddRange(intentoryMovements);
            context.BranchProducts.UpdateRange(productsUpdated);
            context.AuditHistories.Add(auditHistory);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Sale>> GetSalesByBranchAsync(Guid id, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = context.Sales
                .AsQueryable();
            return await query
                .Include(s => s.Branch)
                .Include(s => s.Seller)
                .Include(s => s.Customer)
                .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                .Where(s => s.BranchId == id)
                .FiltersSales(fromDate, toDate)
                .OrderByDescending(b => b.Date)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<BranchProduct?> GetBranchProductByBranchIdAndProductIdAsync(Guid? branchId, int productId)
        {
            return await context.BranchProducts
                .Include(bp => bp.Product)
                .FirstOrDefaultAsync(bp => branchId.HasValue && bp.BranchId == branchId && bp.ProductId == productId);
        }

        public async Task AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts)
        {
            context.BranchProducts.AddRange(branchProducts);
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Product>> GetProductsDoesntExistByBranchAsync(Guid id, int page, int pageSize)
        {
            var query = context.Products
                .AsQueryable(); ;
            return await query
                .Where(p => !context.BranchProducts.Any(bp => bp.BranchId == id && bp.ProductId == p.Id))
                .Include(p => p.Measure)
                .Include(p => p.Category)
                .ToPaginatedListAsync(page, pageSize);
        }
    }
}
