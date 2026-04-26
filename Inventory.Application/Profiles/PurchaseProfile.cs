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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider.Name))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer.Name))
                .ForMember(dest => dest.PurchaseDetails, opt => opt.MapFrom(src => src.PurchaseDetails));
            CreateMap<PurchaseDetail, PurchaseDetailResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}