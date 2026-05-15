using AutoMapper;
using Inventory.Application.DataTransferObjects.BranchProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class BranchProductProfile : Profile
    {
        public BranchProductProfile()
        {
            CreateMap<BranchProduct, BranchProductResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Product.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Product.Category.Description));
            CreateMap<Sale, SaleResponse>()
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller.Name))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Name : null));
             CreateMap<SaleDetail, SaleDetailResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.Name));
        }
    }
}
