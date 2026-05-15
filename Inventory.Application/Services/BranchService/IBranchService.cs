using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Services.BranchService
{
    public interface IBranchService
    {
        Task<PaginatedList<BranchResponse>> GetBranchesAsync(BranchSearchParams searchParams, Guid businessId);
        Task<BranchResponse> GetBranchByIdAsync(Guid id, Guid businessId);
        Task<BranchResponse> CreateBranchAsync(BranchRequest request, Guid businessId);
        Task UpdateBranchAsync(Guid id, BranchRequest request, Guid businessId);
        Task UpdateBranchProductAsync(Guid id, BranchProductRequest request, Guid businessId);
        Task DeleteBranchAsync(Guid id, Guid businessId);
        Task DeleteProductsAsync(Guid branchId, IEnumerable<int> productIds, Guid businessId);
        Task<PaginatedList<BranchProductResponse>> GetProductsByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
        Task CreateSaleAsync(Guid id, SaleRequest request, Guid businessId);
        Task<PaginatedList<SaleResponse>> GetSalesByBranchAsync(Guid id, SaleSearchParams searchParams, Guid businessId);
        Task AddProductsToBranchAsync(Guid id, IEnumerable<BranchProductRequest> request, Guid businessId);
        Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
    }
}
