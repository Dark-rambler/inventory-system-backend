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

        public async Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<Guid> productIds)
        {
            return await context.BranchProducts
                .Include(bp => bp.Product)
                .Where(bp => bp.BranchId == branchId && productIds.Contains(bp.ProductId))
                .ToListAsync();
        }

        public async Task CreateSaleAsync(Sale sale, List<InventoryMovement> intentoryMovements, List<BranchProduct> productsUpdated)
        {
            context.Sales.Add(sale);
            context.InventoryMovements.AddRange(intentoryMovements);
            context.BranchProducts.UpdateRange(productsUpdated);
            await context.SaveChangesAsync();
        }
    }
}
