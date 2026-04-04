using FluentValidation;
using Inventory.Application.Common.Validations;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.Profiles;
using Inventory.Application.Services.AuthService;
using Inventory.Application.Services.BranchService;
using Inventory.Application.Services.CategoryService;
using Inventory.Application.Services.ProductService;
using Inventory.Application.Services.UserService;
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
            typeof(WarehouseProfile),
            typeof(UserProfile),
            typeof(BranchProductProfile));
        services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidation>();
        services.AddScoped<IValidator<ProductRequest>, ProductRequestValidation>();
        services.AddScoped<IValidator<BranchRequest>, BranchRequestValidation>();
        services.AddScoped<IValidator<WarehouseRequest>, WarehouseRequestValidation>();
        services.AddScoped<IValidator<UserRequest>, UserRequestValidation>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
