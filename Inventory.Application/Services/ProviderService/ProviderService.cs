using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.ProviderDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.ProviderService
{
    public class ProviderService(IProviderRepository repository, IMapper mapper, IValidator<ProviderRequest> validator) : IProviderService
    {
        public async Task<PaginatedList<ProviderResponse>> GetProvidersAsync(ProviderSearchParams searchParams, Guid businessId)
        {
            var providers = await repository.GetProvidersAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<ProviderResponse>(
                mapper.Map<List<ProviderResponse>>(providers.Items),
                providers.TotalCount,
                providers.PageIndex,
                providers.PageSize
            );
        }

        public async Task<ProviderResponse> GetProviderByIdAsync(Guid id, Guid businessId)
        {
            return mapper.Map<ProviderResponse>(await FindProviderById(id, businessId));
        }

        public async Task<ProviderResponse> CreateProviderAsync(ProviderRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var provider = mapper.Map<Provider>(request);
            provider.BusinessId = businessId;
            return mapper.Map<ProviderResponse>(await repository.CreateProviderAsync(provider));
        }

        public async Task UpdateProviderAsync(Guid id, ProviderRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateProviderAsync(mapper.Map(request, await FindProviderById(id, businessId)));
        }

        public async Task DeleteProviderAsync(Guid id, Guid businessId)
        {
            await repository.DeleteProviderAsync(await FindProviderById(id, businessId));
        }

        private async Task<Provider> FindProviderById(Guid id, Guid businessId)
        {
            return await repository.GetProviderByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Provider with id {id} doesn't exist");
        }
    }
}