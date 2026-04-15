using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.AuditHistoryDto;

namespace Inventory.Application.Services.AuditHistoryService
{
    public class AuditHistoryService(IAuditHistoryRepository repository, IMapper mapper) : IAuditHistoryService
    {
        public async Task<PaginatedList<AuditHistoryResponse>> GetAuditHistoriesAsync(AuditHistorySearchParams searchParams)
        {
            var auditHistories = await repository.GetAuditHistoriesAsync(
                searchParams.User,
                searchParams.Action,
                searchParams.Entity,
                searchParams.FromDate,
                searchParams.ToDate,
                searchParams.Page,
                searchParams.PageSize);
            return new PaginatedList<AuditHistoryResponse>(
                mapper.Map<List<AuditHistoryResponse>>(auditHistories.Items),
                auditHistories.TotalCount,
                auditHistories.PageIndex,
                auditHistories.PageSize
            );
        }
    }
}