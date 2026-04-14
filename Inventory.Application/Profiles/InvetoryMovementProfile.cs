using AutoMapper;
using Inventory.Application.DataTransferObjects.InventoryMovementDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class InvetoryMovementProfile : Profile
    {
        public InvetoryMovementProfile()
        {
            CreateMap<InventoryMovement, InventoryMovementResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.FromWarehouse, opt => opt.MapFrom(src => src.FromWarehouse != null ? src.FromWarehouse.Name : string.Empty))
                .ForMember(dest => dest.FromBranch, opt => opt.MapFrom(src => src.FromBranch != null ? src.FromBranch.Name : string.Empty))
                .ForMember(dest => dest.ToWarehouse, opt => opt.MapFrom(src => src.ToWarehouse != null ? src.ToWarehouse.Name : string.Empty))
                .ForMember(dest => dest.ToBranch, opt => opt.MapFrom(src => src.ToBranch != null ? src.ToBranch.Name : string.Empty));

            CreateMap<InventoryMovementRequest, InventoryMovement>();
        }
    }
}
