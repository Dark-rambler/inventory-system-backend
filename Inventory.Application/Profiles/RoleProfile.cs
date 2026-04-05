using AutoMapper;
using Inventory.Application.DataTransferObjects.RoleDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleResponse>();
        }
    }
}