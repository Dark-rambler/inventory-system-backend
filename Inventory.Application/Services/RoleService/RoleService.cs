using AutoMapper;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.RoleDto;

namespace Inventory.Application.Services.RoleService
{
    public class RoleService(IRoleRepository repository, IMapper mapper) : IRoleService
    {
        public async Task<List<RoleResponse>> GetRolesAsync()
        {
            return mapper.Map<List<RoleResponse>>(await repository.GetRolesAsync());
        }
    }
}