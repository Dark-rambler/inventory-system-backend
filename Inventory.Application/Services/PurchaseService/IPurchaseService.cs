using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;

namespace Inventory.Application.Services.PurchaseService
{
    public interface IPurchaseService
    {
        Task CreatePurchaseAsync(PurchaseRequest request, Guid businessId);
        Task<PaginatedList<PurchaseResponse>> GetPurchasesAsync(PurchaseSearchParams searchParams, Guid businessId);
    }
}
