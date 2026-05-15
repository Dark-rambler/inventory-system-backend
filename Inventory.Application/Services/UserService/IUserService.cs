using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;

namespace Inventory.Application.Services.UserService
{
    public interface IUserService
    {
        Task<PaginatedList<UserResponse>> GetUsersAsync(UserSearchParams searchParams, Guid businessId);
        Task<UserResponse> GetUserByIdAsync(Guid id, Guid businessId);
        Task<UserResponse> CreateUserAsync(UserRequest request, Guid businessId);
        Task UpdateUserAsync(Guid id, UserRequest request, Guid businessId);
        Task DeleteUserAsync(Guid id, Guid businessId);
    }
}
