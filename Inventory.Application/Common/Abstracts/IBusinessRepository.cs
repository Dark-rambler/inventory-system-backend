using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IBusinessRepository
    {
        Task<Business> CreateBusinessAsync(Business business);
        Task<PaginatedList<Business>> GetBusinessesAsync(string? name, int page, int pageSize);
    }
}
