using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.AuditHistoryDto;

namespace Inventory.Application.Services.AuditHistoryService
{
    public interface IAuditHistoryService
    {
        Task<PaginatedList<AuditHistoryResponse>> GetAuditHistoriesAsync(AuditHistorySearchParams searchParams);
    }
}