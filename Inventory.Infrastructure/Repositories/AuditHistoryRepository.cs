using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Domain.Enum;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;

namespace Inventory.Infrastructure.Repositories
{
    public class AuditHistoryRepository(InventoryDbContext context) : IAuditHistoryRepository
    {
        public async Task<PaginatedList<AuditHistory>> GetAuditHistoriesAsync(Guid businessId, DateTime? fromDate, DateTime? toDate, int page, int pageSize) =>
            await context.AuditHistories.AsQueryable()
                .Where(a => a.BusinessId == businessId)
                .OrderByDescending(a => a.CreatedAt)
                .FiltersAuditHistory(fromDate, toDate)
                .ToPaginatedListAsync(page, pageSize);
    }
}