using Inventory.Application.DataTransferObjects.RoleDto;

namespace Inventory.Application.Services.RoleService
{
    public interface IRoleService
    {
        Task<List<RoleResponse>> GetRolesAsync();
    }
}