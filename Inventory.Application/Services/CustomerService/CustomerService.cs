using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.CustomerService
{
    public class CustomerService(ICustomerRepository repository, IMapper mapper, IValidator<CustomerRequest> validator) : ICustomerService
    {
        public async Task<PaginatedList<CustomerResponse>> GetCustomersAsync(CustomerSearchParams searchParams, Guid businessId)
        {
            var customers = await repository.GetCustomersAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<CustomerResponse>(
                mapper.Map<List<CustomerResponse>>(customers.Items),
                customers.TotalCount,
                customers.PageIndex,
                customers.PageSize
            );
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid id, Guid businessId)
        {
            return mapper.Map<CustomerResponse>(await FindCustomerById(id, businessId));
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var customer = mapper.Map<Customer>(request);
            customer.BusinessId = businessId;
            return mapper.Map<CustomerResponse>(await repository.CreateCustomerAsync(customer));
        }

        public async Task UpdateCustomerAsync(Guid id, CustomerRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateCustomerAsync(mapper.Map(request, await FindCustomerById(id, businessId)));
        }

        private async Task<Customer> FindCustomerById(Guid id, Guid businessId)
        {
            return await repository.GetCustomerByIdAsync(id, businessId) ?? throw new KeyNotFoundException($"Customer with id {id} doesn't exist");
        }
    }
}
