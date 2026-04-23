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
        public async Task<PaginatedList<AuditHistory>> GetAuditHistoriesAsync(Guid? user, EnumAction? action, EnumEntity? entity, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = context.AuditHistories.AsQueryable();
            return await query
                .OrderByDescending(a => a.CreatedAt)
                .FiltersAuditHistory(user, action, entity, fromDate, toDate)
                .ToPaginatedListAsync(page, pageSize);
        }
    }
}