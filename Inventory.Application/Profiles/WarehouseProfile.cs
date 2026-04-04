using AutoMapper;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class WarehouseProfile : Profile
    {
        public WarehouseProfile()
        {
            CreateMap<WarehouseRequest, Warehouse>();
            CreateMap<Warehouse, WarehouseResponse>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.City,
                opt => opt.MapFrom(src => src.Location.City));
        }
    }
}
