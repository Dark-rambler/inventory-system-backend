using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface ICustomerRepository
    {
        Task<PaginatedList<Customer>> GetCustomersAsync(string? name, int page, int pageSize);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Guid id,Customer customer);
        Task DeleteCustomerAsync(Customer customer);
    }
}