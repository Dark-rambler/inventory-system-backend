using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class CustomerRepository(InventoryDbContext context) : ICustomerRepository
    {
        public async Task<PaginatedList<Customer>> GetCustomersAsync(Guid businessId, string? name, int page, int pageSize) =>
            await context.Customers
                .AsQueryable()
                .Where(c => c.BusinessId == businessId)
                .OrderByDescending(b => b.CreatedAt)
                .FiltersCustomer(name)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, Guid businessId) =>
            await context.Customers
                .FirstOrDefaultAsync(c => c.Id == id && c.BusinessId == businessId);

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return await context.Customers
                .FirstAsync(c => c.Id == customer.Id);
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            customer.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }
    }
}
