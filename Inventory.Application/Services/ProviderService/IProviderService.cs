using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProviderDto;

namespace Inventory.Application.Services.ProviderService
{
    public interface IProviderService
    {
        Task<PaginatedList<ProviderResponse>> GetProvidersAsync(ProviderSearchParams searchParams);
        Task<ProviderResponse> GetProviderByIdAsync(Guid id);
        Task<ProviderResponse> CreateProviderAsync(ProviderRequest request);
        Task UpdateProviderAsync(Guid id, ProviderRequest request);
        Task DeleteProviderAsync(Guid id);
    }
}