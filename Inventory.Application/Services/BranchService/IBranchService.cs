using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.Services.BranchService
{
    public interface IBranchService
    {
        Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams);
        Task<BranchResponse> GetBranchByIdAsync(Guid id);
        Task<BranchResponse> CreateBranchAsync(BranchRequest request);
        Task UpdateBranchAsync(Guid id, BranchRequest request);
        Task DeleteBranchAsync(Guid id);
    }
}
