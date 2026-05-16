using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BranchProductDto;

namespace Inventory.Application.Services.SaleService
{
    public interface ISaleService
    {
        Task CreateSaleAsync(Guid id, SaleRequest request, Guid businessId);
        Task<PaginatedList<SaleResponse>> GetSalesByBranchAsync(Guid id, SaleSearchParams searchParams, Guid businessId);
    }
}
