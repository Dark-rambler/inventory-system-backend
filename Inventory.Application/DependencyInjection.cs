using FluentValidation;
using Inventory.Application.Common.Validations;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.Profiles;
using Inventory.Application.Services.CategoryService;
using Inventory.Application.Services.ProductService;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ProductProfile), typeof(CategoryProfile));
        services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidation>();
        services.AddScoped<IValidator<ProductRequest>, ProductRequestValidation>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
