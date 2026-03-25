# AGENTS.md - Inventory Management System

## Project Overview

This is a .NET 10.0 inventory management API built with Clean Architecture. The solution contains four projects:

- **Inventory.API** - ASP.NET Core Web API
- **Inventory.Application** - Application services, DTOs, validations, and interfaces
- **Inventory.Domain** - Domain entities
- **Inventory.Infrastructure** - Repositories, DbContext, and EF Core configuration

## Build & Run Commands

```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build Inventory.API/Inventory.API.csproj

# Run API (from Inventory.API folder)
cd Inventory.API && dotnet run

# Run with development profile
dotnet run --launch-profile Development
```

## Database Commands

```bash
# Add migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

Note: Run these from Infrastructure folder with `dotnet ef` installed globally or use `dotnet tool install --global dotnet-ef`.

## Test Commands

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test -v n

# Run a single test
dotnet test --filter "FullyQualifiedName~TestNamespace.TestClassName.TestMethodName"

# Run tests in specific project
dotnet test Inventory.Tests/Inventory.Tests.csproj
```

## Code Style Guidelines

### General Conventions

- **Target Framework**: .NET 10.0
- **Language Version**: Latest (C# 12+)
- **Nullable**: Enabled
- **Implicit Usings**: Enabled
- **Indentation**: 2 spaces

### Namespace & File Organization

```csharp
// File-scoped namespace matching folder structure
namespace Inventory.Application.Services.ProductService
```

### Naming Conventions

- **Classes/Interfaces**: PascalCase (e.g., `ProductService`, `IProductRepository`)
- **Methods**: PascalCase (e.g., `GetProductsAsync`)
- **Properties**: PascalCase (e.g., `ProductName`)
- **Private fields**: camelCase (if used)
- **DTOs**: Follow pattern `{EntityName}Request`, `{EntityName}Response`, `{EntityName}SearchParams`

### Project Structure Patterns

```
Inventory.Application/
├── Common/
│   ├── Abstracts/        # Repository interfaces
│   ├── Validations/      # FluentValidation validators
│   ├── Pagination/       # Pagination utilities
│   └── Extensions/       # Extension methods
├── DataTransferObjects/
│   └── {EntityName}Dto/  # Request, Response, SearchParams
├── Services/
│   └── {EntityName}Service/  # Service + interface
├── Profiles/             # AutoMapper profiles
└── DependencyInjection.cs
```

### Code Patterns

#### Primary Constructors (C# 12+)
```csharp
public class ProductController(IProductService service) : ControllerBase
{
    // Controller implementation
}

public class ProductService(
    IProductRepository repository, 
    IMapper mapper, 
    IValidator<ProductRequest> validator) : IProductService
{
    // Service implementation
}
```

#### Repository Pattern
- Interfaces defined in `Inventory.Application/Common/Abstracts/`
- Implementations in `Inventory.Infrastructure/Repositories/`
- Use soft deletes (set `IsDeleted = true` instead of removing records)

#### Service Layer
- Async methods with `Task<T>` return types
- Use `ValidateAndThrowAsync` from FluentValidation
- Throw `KeyNotFoundException` for missing entities

```csharp
public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
{
    await validator.ValidateAndThrowAsync(request);
    return mapper.Map<ProductResponse>(await repository.CreateProductAsync(mapper.Map<Product>(request)));
}
```

### Error Handling

- Use `ExceptionHandlingMiddleware` for centralized error handling
- Throw specific exceptions: `KeyNotFoundException`, `ArgumentException`, `ValidationException`
- Return appropriate HTTP status codes (400, 401, 404, 500)

### Validation

Use FluentValidation with clear, descriptive messages:

```csharp
RuleFor(p => p.Name)
    .NotEmpty().WithMessage("Product name is required.")
    .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
```

### AutoMapper

Define profiles in `Inventory.Application/Profiles/`:

```csharp
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
    }
}
```

### Dependency Injection

- Register services in respective `DependencyInjection.cs` files
- Application layer: `services.AddScoped<IProductService, ProductService>()` in `Inventory.Application/DependencyInjection.cs`
- Infrastructure layer: `services.AddDbContext<InventoryDbContext>()` in `Inventory.Infrastructure/DependencyInjection.cs`

### API Controllers

- Use `[ApiController]` attribute
- Route: `[Route("api/[controller]")]`
- Use constructor injection
- Return appropriate IActionResult types

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromQuery] ProductSearchParams searchParams)
    {
        return Ok(await service.GetProductsAsync(searchParams));
    }
}
```

### DTO Guidelines

- **Request DTOs**: Input validation with FluentValidation
- **Response DTOs**: Read-only style, use `default!` for navigation properties
- **SearchParams**: Query string parameters with sensible defaults (Page = 1, PageSize = 10)

### Entity Guidelines

- Use `Guid` for IDs: `public Guid Id { get; set; } = Guid.NewGuid();`
- Include soft delete: `public bool IsDeleted { get; set; } = false;`
- Include audit fields: `CreatedAt`, `UpdatedAt` when needed
- Navigation properties: `public Category Category { get; set; } = default!;`

### Imports

Organize imports in this order:
1. System namespaces
2. Third-party libraries (AutoMapper, FluentValidation)
3. Project namespaces (Application, Domain, Infrastructure)

## Database Configuration

- **Provider**: PostgreSQL (Npgsql)
- **ORM**: Entity Framework Core 10.0.1
- **Connection**: Configured in `appsettings.json` under `ConnectionStrings.DefaultConnection`
- **Migrations**: Located in `Inventory.Infrastructure/Migrations/`

## Testing

When adding tests:
- Follow naming: `{ClassName}Tests`
- Use ` Xunit` or `NUnit` (check existing test project)
- Test service logic, validation, and mapping
- Use in-memory database for repository tests

## Common Issues

- **Migration conflicts**: Ensure you're in the correct project folder when running EF commands
- **Circular dependencies**: Domain should have no dependencies; Application depends on Domain; Infrastructure depends on Application and Domain; API depends on Application and Infrastructure
- **Null reference warnings**: Use `= default!;` for navigation properties that are set by EF Core
