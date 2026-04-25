using AutoMapper;
using Inventory.Application.DataTransferObjects.ProviderDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            CreateMap<ProviderRequest, Provider>();
            CreateMap<Provider, ProviderResponse>();
        }
    }
}