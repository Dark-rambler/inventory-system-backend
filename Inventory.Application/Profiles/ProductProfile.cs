using AutoMapper;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Category.Description));
        }
    }
}
