using FluentValidation;
using Inventory.Application.Common.Validations;
using Inventory.Application.DataTransferObjects.BranchDto;
using Inventory.Application.DataTransferObjects.CategoryDto;
using Inventory.Application.DataTransferObjects.CustomerDto;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Application.DataTransferObjects.ProviderDto;
using Inventory.Application.DataTransferObjects.PurchaseDto;
using Inventory.Application.DataTransferObjects.UserDto;
using Inventory.Application.DataTransferObjects.WarehouseDto;
using Inventory.Application.Profiles;
using Inventory.Application.Services.AuthService;
using Inventory.Application.Services.AuditHistoryService;
using Inventory.Application.Services.BranchService;
using Inventory.Application.Services.CategoryService;
using Inventory.Application.Services.CustomerService;
using Inventory.Application.Services.InventoryMovementService;
using Inventory.Application.Services.InventoryMovementService.InventoryMovementStrategy;
using Inventory.Application.Services.MeasureService;
using Inventory.Application.Services.ProductService;
using Inventory.Application.Services.ProviderService;
using Inventory.Application.Services.PurchaseService;
using Inventory.Application.Services.RoleService;
using Inventory.Application.Services.UserService;
using Inventory.Application.Services.WarehouseService;
using Inventory.Application.Common.Abstracts;
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
            typeof(BranchProductProfile),
            typeof(RoleProfile),
            typeof(MeasureProfile),
            typeof(InventoryMovementProfile),
            typeof(MeasureProfile),
            typeof(WarehouseProductProfile),
            typeof(AuditHistoryProfile),
            typeof(CustomerProfile),
            typeof(ProviderProfile),
            typeof(PurchaseProfile));
        services.AddScoped<IValidator<CategoryRequest>, CategoryRequestValidation>();
        services.AddScoped<IValidator<ProductRequest>, ProductRequestValidation>();
        services.AddScoped<IValidator<BranchRequest>, BranchRequestValidation>();
        services.AddScoped<IValidator<WarehouseRequest>, WarehouseRequestValidation>();
        services.AddScoped<IValidator<UserRequest>, UserRequestValidation>();
        services.AddScoped<IValidator<CustomerRequest>, CustomerRequestValidation>();
        services.AddScoped<IValidator<ProviderRequest>, ProviderRequestValidation>();
        services.AddScoped<IValidator<PurchaseRequest>, PurchaseRequestValidation>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IMeasureService, MeasureService>();
        services.AddScoped<IInventoryMovementService, InventoryMovementService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuditHistoryService, AuditHistoryService>();
        services.AddScoped<IProviderService, ProviderService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IInventoryMovementStrategy, EntryMovementStrategy>();
        services.AddScoped<IInventoryMovementStrategy, ExitMovementStrategy>();
        services.AddScoped<IInventoryMovementStrategy, TransferMovementStrategy>();
        services.AddScoped<MovementStrategyResolver>();
        return services;
    }
}
