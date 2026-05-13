using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;

namespace Inventory.Application.Services.UserService
{
    public interface IUserService
    {
        Task<PaginatedList<UserResponse>> GetUsersAsync(UserSearchParams searchParams, Guid businessId);
        Task<UserResponse> GetUserByIdAsync(Guid id);
        Task<UserResponse> CreateUserAsync(UserRequest request, Guid businessId);
        Task UpdateUserAsync(Guid id, UserRequest request);
        Task DeleteUserAsync(Guid id);
    }
}
