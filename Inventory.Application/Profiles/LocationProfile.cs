using AutoMapper;
using Inventory.Application.DataTransferObjects.LocationDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<LocationRequest, Location>();
            CreateMap<Location, LocationResponse>();
        }
    }
}
