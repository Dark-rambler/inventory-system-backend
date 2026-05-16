using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;

namespace Inventory.Application.Services.BranchService
{
    public interface IBranchService
    {
        Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams, Guid businessId);
        Task<BranchResponse> GetBranchByIdAsync(Guid id, Guid businessId);
        Task<BranchResponse> CreateBranchAsync(BranchRequest request, Guid businessId);
        Task UpdateBranchAsync(Guid id, BranchRequest request, Guid businessId);
        Task DeleteBranchAsync(Guid id, Guid businessId);
    }
}
