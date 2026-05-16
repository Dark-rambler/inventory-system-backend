using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Application.DataTransferObjects.ProductDto;

namespace Inventory.Application.Services.BranchProductService
{
    public interface IBranchProductService
    {
        Task<PaginatedList<BranchProductResponse>> GetProductsByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
        Task AddProductsToBranchAsync(Guid id, IEnumerable<BranchProductRequest> request, Guid businessId);
        Task DeleteProductsAsync(Guid branchId, IEnumerable<int> productIds, Guid businessId);
        Task<PaginatedList<ProductResponse>> GetProductsDoesntExistByBranchAsync(Guid id, ProductSearchParams searchParams, Guid businessId);
        Task UpdateBranchProductAsync(Guid id, BranchProductRequest request, Guid businessId);
    }
}
