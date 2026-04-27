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
        Task<IEnumerable<BranchProduct>> GetBranchProductsByProductIdsAsync(Guid branchId, IEnumerable<Guid> productIds);
        Task CreateSaleAsync(Sale sale, List<InventoryMovement> intentoryMovements, List<BranchProduct> productsUpdated, AuditHistory auditHistory);
        Task<PaginatedList<Sale>> GetSalesByBranchAsync(Guid id, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        Task<BranchProduct?> GetBranchProductByBranchIdAndProductIdAsync(Guid? branchId, Guid productId);
        Task AddProductsToBranchAsync(IEnumerable<BranchProduct> branchProducts);
        Task<IEnumerable<Product>> GetProductsDoesntExistByBranchAsync(Guid id);
    }
}
