using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;

namespace Inventory.Application.Common.Abstracts
{
    public interface IAuditHistoryRepository
    {
        Task<PaginatedList<AuditHistory>> GetAuditHistoriesAsync(Guid? user, EnumAction? action, EnumEntity? entity, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
    }
}