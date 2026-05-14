using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services.UserService
{
    public class UserService(
        IUserRepository repository,
        IMapper mapper,
        IValidator<UserRequest> validator,
        IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<PaginatedList<UserResponse>> GetUsersAsync(UserSearchParams searchParams, Guid businessId)
        {
            var paginatedUsers = await repository.GetUsersAsync(businessId, searchParams.Name, searchParams.Page, searchParams.PageSize);
            return new PaginatedList<UserResponse>(
                mapper.Map<List<UserResponse>>(paginatedUsers.Items),
                paginatedUsers.TotalCount,
                paginatedUsers.PageIndex,
                paginatedUsers.PageSize
            );
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid id)
        {
            return mapper.Map<UserResponse>(await FindUserById(id));
        }

        public async Task<UserResponse> CreateUserAsync(UserRequest request, Guid businessId)
        {
            await validator.ValidateAndThrowAsync(request);
            var user = mapper.Map<User>(request);
            user.Password = passwordHasher.Hash(request.Password);
            user.BusinessId = businessId;
            return mapper.Map<UserResponse>(await repository.CreateUserAsync(user));
        }

        public async Task UpdateUserAsync(Guid id, UserRequest request)
        {
            await validator.ValidateAndThrowAsync(request);
            await repository.UpdateUserAsync(mapper.Map(request, await FindUserById(id)));
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await repository.DeleteUserAsync(await FindUserById(id));
        }

        private async Task<User> FindUserById(Guid id)
        {
            return await repository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"User with id {id} doesn't exist");
        }
    }
}
