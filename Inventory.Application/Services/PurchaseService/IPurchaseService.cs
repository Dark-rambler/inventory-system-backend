using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.PurchaseDto;

namespace Inventory.Application.Services.PurchaseService
{
    public interface IPurchaseService
    {
        Task CreatePurchaseAsync(PurchaseRequest request, Guid user);
        Task<PaginatedList<PurchaseResponse>> GetPurchasesAsync(PurchaseSearchParams searchParams);
    }
}