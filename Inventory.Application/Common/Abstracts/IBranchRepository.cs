using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IBranchRepository
    {
        Task<PaginatedList<Branch>> GetBranchesAsync(string? name, int page, int pageSize);
        Task<Branch?> GetBranchByIdAsync(Guid id);
        Task<Branch> CreateBranchAsync(Branch branch);
        Task UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(Branch branch);
        Task<PaginatedList<BranchProduct>> GetProductsByBranchAsync(Guid id, string? name, int page, int pageSize);
        Task AddStockAsync(Guid branchId, Guid productId, int stock);
    }
}
