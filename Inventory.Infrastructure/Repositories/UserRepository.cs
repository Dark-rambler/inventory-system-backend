using Inventory.Application.Common.Abstracts;
using Inventory.Application.Common.Pagination;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Inventory.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class UserRepository(InventoryDbContext context) : IUserRepository
    {
        public async Task<PaginatedList<User>> GetUsersAsync(string? name, int page, int pageSize)
        {
            var query = context.Users
                .AsQueryable();
            return await query
                .OrderByDescending(u => u.CreatedAt)
                .FiltersUser(name)
                .ToPaginatedListAsync(page, pageSize);
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return await context.Users
                .FirstAsync(u => u.Id == user.Id);
        }

        public async Task UpdateUserAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
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
