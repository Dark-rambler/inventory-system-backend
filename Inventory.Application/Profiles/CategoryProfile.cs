using AutoMapper;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Domain.Entities;

namespace Inventory.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            // Define your AutoMapper configurations here
        }
    }
}
