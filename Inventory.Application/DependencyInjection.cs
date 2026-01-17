using FluentValidation;
using Inventory.Application.Common.Validations;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.Profiles;
using Inventory.Application.Services.BranchService;
using Inventory.Application.Services.CategoryService;
using Inventory.Application.Services.ProductService;
using Inventory.Application.Services.WarehouseService;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { },
            typeof(ProductProfile),
            typeof(CategoryProfile),
            typeof(BranchProfile),
            typeof(WarehouseProfile));
        services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidation>();
        services.AddScoped<IValidator<ProductRequest>, ProductRequestValidation>();
        services.AddScoped<IValidator<BranchRequest>, BranchRequestValidation>();
        services.AddScoped<IValidator<WarehouseRequest>, WarehouseRequestValidation>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        return services;
    }
}
