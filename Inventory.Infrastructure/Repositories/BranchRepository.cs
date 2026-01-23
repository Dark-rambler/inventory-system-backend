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
        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            context.Branches.Add(branch);
            await context.SaveChangesAsync();
            return await context.Branches
                .FirstAsync(b => b.Id == branch.Id);
        }

        public async Task DeleteBranchAsync(Branch branch)
        {
            branch.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Branch>> GetBranchesAsync(string? name, int page, int pageSize)
        {
            var query = context.Branches
                .AsQueryable();
            return await query
                .OrderByDescending(b => b.CreatedAt)
                .FiltersBranch(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<Branch?> GetBranchByIdAsync(Guid id)
        {
            return await context.Branches
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            context.Branches.Update(branch);
            await context.SaveChangesAsync();
        }
    }
}
