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
        public async Task<PaginatedList<ProviderResponse>> GetProvidersAsync(ProviderSearchParams searchParams)
        {
            var providers = await repository.GetProvidersAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<ProviderResponse>(
                mapper.Map<List<ProviderResponse>>(providers.Items),
                providers.TotalCount,
                providers.PageIndex,
                providers.PageSize
            );
        }

        public async Task<ProviderResponse> GetProviderByIdAsync(Guid id)
        {
            return mapper.Map<ProviderResponse>(await FindProviderById(id));
        }

        public async Task<ProviderResponse> CreateProviderAsync(ProviderRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<ProviderResponse>(await repository.CreateProviderAsync(mapper.Map<Provider>(request)));
        }

        public async Task UpdateProviderAsync(Guid id, ProviderRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateProviderAsync(mapper.Map(request, await FindProviderById(id)));
        }

        public async Task DeleteProviderAsync(Guid id)
        {
            await repository.DeleteProviderAsync(await FindProviderById(id));
        }

        private async Task<Provider> FindProviderById(Guid id)
        {
            return await repository.GetProviderByIdAsync(id) ?? throw new KeyNotFoundException($"Provider with id {id} doesn't exist");
        }
    }
}