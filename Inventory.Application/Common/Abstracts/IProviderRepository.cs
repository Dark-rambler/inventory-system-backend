using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IProviderRepository
    {
        Task<PaginatedList<Provider>> GetProvidersAsync(Guid businessId, string? name, int page, int pageSize);
        Task<Provider?> GetProviderByIdAsync(Guid id, Guid businessId);
        Task<Provider> CreateProviderAsync(Provider provider);
        Task UpdateProviderAsync(Provider provider);
        Task DeleteProviderAsync(Provider provider);
    }
}