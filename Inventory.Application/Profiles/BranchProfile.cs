using AutoMapper;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<BranchRequest, Branch>();
            CreateMap<Branch, BranchResponse>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.Address))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Location.City));
        }
    }
}
