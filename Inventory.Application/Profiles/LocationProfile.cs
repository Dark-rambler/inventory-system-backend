using AutoMapper;
using Inventory.Application.DataTransferObjects.Location;
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
