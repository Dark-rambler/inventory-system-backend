using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class BusinessRepository(InventoryDbContext context) : IBusinessRepository
    {
        public async Task<Business> CreateBusinessAsync(Business business)
        {
            context.Businesses.Add(business);
            await context.SaveChangesAsync();
            return await context.Businesses.FirstAsync(b => b.Id == business.Id);
        }

        public async Task<PaginatedList<Business>> GetBusinessesAsync(string? name, int page, int pageSize) =>
            await context.Businesses
                .AsQueryable()
                .OrderBy(b => b.Name)
                .FiltersBusiness(name)
                .ToPaginatedListAsync(page, pageSize);
    }
}
