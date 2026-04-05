using Inventory.Application.Common.Abstracts;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class RoleRepository(InventoryDbContext context) : IRoleRepository
    {
        public async Task<List<Role>> GetRolesAsync()
        {
            return await context.Roles.ToListAsync();
        }
    }
}