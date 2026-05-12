using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;

namespace Inventory.Application.Services.PurchaseService
{
    public interface IPurchaseService
    {
        Task CreatePurchaseAsync(PurchaseRequest request);
        Task<PaginatedList<PurchaseResponse>> GetPurchasesAsync(PurchaseSearchParams searchParams);
    }
}
