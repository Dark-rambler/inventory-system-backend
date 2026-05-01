using AutoMapper;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile()
        {
            CreateMap<Purchase, PurchaseResponse>()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider.Name))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer.Name));
            CreateMap<PurchaseDetail, PurchaseDetailResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}