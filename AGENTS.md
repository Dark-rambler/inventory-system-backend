# AGENTS.md - Developer Guidelines

## Project Overview

.NET 10 inventory management system following Clean Architecture with 4 projects:
- **Inventory.Domain**: Entities and domain models
- **Inventory.Application**: Services, DTOs, validators, interfaces
- **Inventory.Infrastructure**: EF Core, repositories, database context
- **Inventory.API**: Controllers, middleware, entry point

## Build & Development Commands

### Build & Run
```bash
dotnet build
dotnet run --project Inventory.API/Inventory.API.csproj --configuration Debug
```

### Linting
```bash
dotnet build --no-incremental
```

### Testing
```bash
dotnet new xunit -n Inventory.Tests    # Create test project (if needed)
dotnet test                           # Run all tests
dotnet test --filter "FullyQualifiedName~TestMethodName"  # Run single test
```

## Architecture

- **Clean Architecture**: Domain -> Application -> Infrastructure -> API
- **Repository Pattern**: interfaces in `Inventory.Application/Common/Abstracts/`, implementations in `Inventory.Infrastructure/Repositories/`
- **CQRS-lite**: Separate request/response DTOs, services return DTOs, not entities

### Project Structure
```
Inventory.Application/
├── Common/Abstracts/       # Repository interfaces
├── Common/Validations/     # FluentValidation validators
├── DataTransferObjects/[Entity]Dto/  # Request, Response, SearchParams
├── Services/[Entity]Service/         # IEntityService, EntityService
└── Profiles/                        # AutoMapper profiles

Inventory.Infrastructure/
├── Context/                # EF Core DbContext
├── Repositories/           # Repository implementations
└── Migrations/            # EF Core migrations
```

## Code Style

### Naming Conventions
| Element | Convention | Example |
|---------|------------|---------|
| Files/Classes/Interfaces | PascalCase | `ProductService.cs`, `IProductRepository` |
| Methods/Properties | PascalCase | `GetProductsAsync`, `PageIndex` |
| Private fields | camelCase | `_repository` |
| Parameters | camelCase | `searchParams` |
| DTOs | Entity + Request/Response | `ProductRequest`, `ProductResponse` |
| Controllers | Entity + Controller | `ProductsController` |

### Imports Order
1. System namespaces (`System`, `System.Collections.Generic`)
2. External packages (`AutoMapper`, `FluentValidation`)
3. Project namespaces alphabetically (`Inventory.Application.*`, `Inventory.Domain.*`, `Inventory.Infrastructure.*`)

### C# Patterns

**Primary Constructor (C# 12+)**:
```csharp
public class ProductService(
    IProductRepository repository,
    IMapper mapper,
    IValidator<ProductRequest> validator
) : IProductService { }
```

**Records for DTOs**:
```csharp
public record ProductResponse(Guid Id, string Name, string Code);
```

**Entity Guidelines**:
- Use `Guid` for all primary keys
- Include soft delete: `bool IsDeleted { get; set; } = false;`
- Use `DateTime` for timestamps: `CreatedAt`, `UpdatedAt`
- Default values in property initializers:
```csharp
public Guid Id { get; set; } = Guid.NewGuid();
public string Name { get; set; } = string.Empty;
ICollection<T> { get; set; } = [];
```

### Error Handling
- Not found: `throw new KeyNotFoundException($"Entity with id {id} not found")`
- Validation: `await validator.ValidateAndThrowAsync(request);`
- Global exception handling via `Inventory.API/Middlewares/ExceptionHandlingMiddleware.cs`

### API Controllers
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductResponse>> GetProduct(Guid id) { }
}
```

## Database

### Migrations
```bash
dotnet ef migrations add [Name] --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```

### Reset Database
```bash
dotnet ef database drop --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef migrations add InitialCreate --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```

## Adding New Entity

1. Create entity in `Inventory.Domain/Entities/`
2. Add DTOs in `Inventory.Application/DataTransferObjects/[Entity]Dto/`
3. Create repository interface in `Inventory.Application/Common/Abstracts/`
4. Implement repository in `Inventory.Infrastructure/Repositories/`
5. Create service interface + implementation in `Inventory.Application/Services/`
6. Create validator in `Inventory.Application/Common/Validations/`
7. Add AutoMapper profile in `Inventory.Application/Profiles/`
8. Create controller in `Inventory.API/Controllers/`
9. Register DI in `Inventory.Application/DependencyInjection.cs`

## Packages
- **AutoMapper** v16.1.1
- **FluentValidation** v12.1.1
- **Swashbuckle.AspNetCore** v10.1.7
