using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class ProviderRepository(InventoryDbContext context) : IProviderRepository
    {
        public async Task<PaginatedList<Provider>> GetProvidersAsync(string? name, int page, int pageSize)
        {
            var query = context.Providers
                .AsQueryable();
            return await query
                .OrderByDescending(b => b.CreatedAt)
                .FiltersProvider(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public Task<Provider?> GetProviderByIdAsync(Guid id)
        {
            return context.Providers
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Provider> CreateProviderAsync(Provider provider)
        {
            context.Providers.Add(provider);
            await context.SaveChangesAsync();
            return await context.Providers
                .FirstAsync(c => c.Id == provider.Id);
        }

        public async Task UpdateProviderAsync(Provider provider)
        {
            provider.UpdatedAt = DateTime.UtcNow;
            context.Providers.Update(provider);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProviderAsync(Provider provider)
        {
            provider.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}