using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IUserRepository
    {
        Task<PaginatedList<User>> GetUsersAsync(Guid businessId, string? name, int page, int pageSize);
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
