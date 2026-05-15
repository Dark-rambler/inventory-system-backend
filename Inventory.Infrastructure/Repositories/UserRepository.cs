using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class UserRepository(InventoryDbContext context, IDateTimeProvider dateTimeProvider) : IUserRepository
    {
        public async Task<PaginatedList<User>> GetUsersAsync(Guid businessId, string? name, int page, int pageSize) =>
            await context.Users
                .AsQueryable()
                .Where(u => u.BusinessId == businessId)
                .Include(u => u.Role)
                .OrderByDescending(u => u.CreatedAt)
                .FiltersUser(name)
                .ToPaginatedListAsync(page, pageSize);

        public async Task<User?> GetUserByIdAsync(Guid id, Guid businessId) =>
            await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && u.BusinessId == businessId);

        public async Task<User?> GetUserByUserNameAsync(string userName) =>
            await context.Users
                .Include(u => u.Role)
                .Include(u => u.Business)
                .FirstOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted);

        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return await context.Users
                .Include(u => u.Role)
                .FirstAsync(u => u.Id == user.Id);
        }

        public async Task UpdateUserAsync(User user)
        {
            user.UpdatedAt = dateTimeProvider.UtcNow;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            user.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}
