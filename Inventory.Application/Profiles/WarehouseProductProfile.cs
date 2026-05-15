using AutoMapper;
using Inventory.Application.DataTransferObjects.WarehouseProductDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class WarehouseProductProfile: Profile
    {
        public WarehouseProductProfile()
        {
            CreateMap<WarehouseProduct, WarehouseProductResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Product.Code))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name))
            .ForMember(dest => dest.CategoryDescription, opt => opt.MapFrom(src => src.Product.Category.Description))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Product.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.Product.UpdatedAt));
        }
    }
}
