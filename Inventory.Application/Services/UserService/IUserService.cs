using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;

namespace Inventory.Application.Services.UserService
{
    public interface IUserService
    {
        Task<PaginatedList<UserResponse>> GetUsersAsync(UserSearchParams searchParams);
        Task<UserResponse> GetUserByIdAsync(Guid id);
        Task<UserResponse> CreateUserAsync(UserRequest request);
        Task UpdateUserAsync(Guid id, UserRequest request);
        Task DeleteUserAsync(Guid id);
    }
}
