using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<PaginatedList<User>> GetUsersAsync(string? name, int page, int pageSize);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
