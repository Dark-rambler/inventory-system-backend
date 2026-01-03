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
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
