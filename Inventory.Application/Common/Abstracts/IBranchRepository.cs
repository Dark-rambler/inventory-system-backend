using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IBranchRepository
    {
        Task<Branch> CreateBranchAsync(Branch branch);
        Task<Branch?> GetBranchByIdAsync(Guid id);
        Task<PaginatedList<Branch>> GetBranchesAsync(string? name, int page, int pageSize);
        Task UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(Branch branch);
    }
}
