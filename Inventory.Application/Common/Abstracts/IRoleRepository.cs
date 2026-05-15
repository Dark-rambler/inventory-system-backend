using Inventory.Domain.Entities;

namespace Inventory.Application.Common.Abstracts
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRolesAsync();
    }
}