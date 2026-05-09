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
        public async Task<PaginatedList<CustomerResponse>> GetCustomersAsync(CustomerSearchParams searchParams)
        {
            var customers = await repository.GetCustomersAsync(searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<CustomerResponse>(
                mapper.Map<List<CustomerResponse>>(customers.Items),
                customers.TotalCount,
                customers.PageIndex,
                customers.PageSize
            );
        }

        public async Task<CustomerResponse> GetCustomerByIdAsync(Guid id)
        {
            return mapper.Map<CustomerResponse>(await FindCustomerById(id));
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<CustomerResponse>(await repository.CreateCustomerAsync(mapper.Map<Customer>(request)));
        }
        public async Task<CustomerResponse> UpdateCustomerAsync(Guid id, CustomerRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            return mapper.Map<CustomerResponse>(await repository.UpdateCustomerAsync(id, mapper.Map<Customer>(request)));

        }


        private async Task<Customer> FindCustomerById(Guid id)
        {
            return await repository.GetCustomerByIdAsync(id) ?? throw new KeyNotFoundException($"Customer with id {id} doesn't exist");
        }
    }
}