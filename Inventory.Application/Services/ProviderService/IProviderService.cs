using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProviderDto;

namespace Inventory.Application.Services.ProviderService
{
    public interface IProviderService
    {
        Task<PaginatedList<ProviderResponse>> GetProvidersAsync(ProviderSearchParams searchParams, Guid businessId);
        Task<ProviderResponse> GetProviderByIdAsync(Guid id, Guid businessId);
        Task<ProviderResponse> CreateProviderAsync(ProviderRequest request, Guid businessId);
        Task UpdateProviderAsync(Guid id, ProviderRequest request, Guid businessId);
        Task DeleteProviderAsync(Guid id, Guid businessId);
    }
}