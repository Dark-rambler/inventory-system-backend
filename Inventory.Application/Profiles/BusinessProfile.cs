using AutoMapper;
using Inventory.Application.DataTransferObjects.BusinessDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<BusinessRequest, Business>();
            CreateMap<Business, BusinessResponse>();
        }
    }
}
