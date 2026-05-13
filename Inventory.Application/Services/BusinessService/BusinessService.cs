using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.BusinessDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.BusinessService
{
    public class BusinessService(
        IBusinessRepository repository,
        IMapper mapper,
        IValidator<BusinessRequest> validator) : IBusinessService
    {
        public async Task<BusinessResponse> CreateBusinessAsync(BusinessRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<BusinessResponse>(
                await repository.CreateBusinessAsync(mapper.Map<Business>(request)));
        }

        public async Task<PaginatedList<BusinessResponse>> GetBusinessesAsync(BusinessSearchParams searchParams)
        {
            var businesses = await repository.GetBusinessesAsync(
                searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<BusinessResponse>(
                mapper.Map<List<BusinessResponse>>(businesses.Items),
                businesses.TotalCount,
                businesses.PageIndex,
                businesses.PageSize);
        }
    }
}
