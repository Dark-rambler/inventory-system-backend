using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.CustomerDto;

namespace Inventory.Application.Services.CustomerService
{
    public interface ICustomerService
    {
        Task<PaginatedList<CustomerResponse>> GetCustomersAsync(CustomerSearchParams searchParams, Guid businessId);
        Task<CustomerResponse> GetCustomerByIdAsync(Guid id);
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, Guid businessId);
        Task UpdateCustomerAsync(Guid id, CustomerRequest request);
    }
}