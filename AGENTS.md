# AGENTS.md - Developer Guidelines

## Project Overview

This is a .NET 10 inventory management system following clean architecture with 4 projects:
- **Inventory.Domain**: Entities and domain models
- **Inventory.Application**: Services, DTOs, validators, interfaces
- **Inventory.Infrastructure**: EF Core, repositories, database context
- **Inventory.API**: Controllers, middleware, entry point

## Build & Development Commands

### Build Solution
```bash
dotnet build
```

### Build Specific Project
```bash
dotnet build Inventory.API/Inventory.API.csproj
```

### Run Application
```bash
dotnet run --project Inventory.API/Inventory.API.csproj
```

### Run in Development (with Swagger)
```bash
dotnet run --project Inventory.API/Inventory.API.csproj --configuration Debug
```

### Linting
This project uses built-in .NET analyzers. To check for issues:
```bash
dotnet build --no-incremental
```

### Testing
Currently no test project exists. To add tests:
```bash
# Create test project
dotnet new xunit -n Inventory.Tests

# Run all tests
dotnet test

# Run single test (use --filter)
dotnet test --filter "FullyQualifiedName~TestMethodName"
```

## Code Style Guidelines

### General Architecture
- Follow **Clean Architecture** principles: Domain -> Application -> Infrastructure -> API
- Use **Repository Pattern**: interfaces in `Inventory.Application/Common/Abstracts/`, implementations in `Inventory.Infrastructure/Repositories/`
- Use **CQRS-lite**: Separate request/response DTOs, services return DTOs, not entities

### Project Structure
```
Inventory.Application/
├── Common/
│   ├── Abstracts/       # Repository interfaces
│   ├── Validations/     # FluentValidation validators
│   ├── Pagination/     # PaginatedList<T>
│   └── Extensions/    # Extension methods
├── DataTransferObjects/
│   └── [Entity]Dto/    # Request, Response, SearchParams
├── Services/
│   └── [Entity]Service/
│       ├── I[Entity]Service.cs
│       └── [Entity]Service.cs
└── Profiles/           # AutoMapper profiles

Inventory.Infrastructure/
├── Context/            # EF Core DbContext
├── Repositories/       # Repository implementations
├── Extensions/         # Query extensions
└── Migrations/        # EF Core migrations
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Files | PascalCase | `ProductService.cs` |
| Classes/Interfaces | PascalCase | `IProductRepository` |
| Methods | PascalCase | `GetProductsAsync` |
| Properties | PascalCase | `PageIndex` |
| Private fields | camelCase | `_repository` |
| Parameters | camelCase | `searchParams` |
| DTOs | Entity + Request/Response | `ProductRequest`, `ProductResponse` |
| Controllers | Entity + Controller | `ProductsController` |

### C# Patterns

#### Dependency Injection
Use primary constructor syntax (C# 12+):
```csharp
public class ProductService(
    IProductRepository repository,
    IMapper mapper,
    IValidator<ProductRequest> validator
) : IProductService
```

#### Use Records for DTOs
```csharp
public record ProductResponse(Guid Id, string Name, string Code);
```

#### Null Handling
- Project has `<Nullable>enable</Nullable>`
- Use null-coalescing and null-conditional operators
- Initialize collections with empty collections: `ICollection<T> { get; set; } = [];`

### Imports

Organize imports in this order:
1. System namespaces (`System`, `System.Collections.Generic`)
2. External packages (`AutoMapper`, `FluentValidation`)
3. Project namespaces (alphabetically)
   - `Inventory.Application.*`
   - `Inventory.Domain.*`
   - `Inventory.Infrastructure.*`

```csharp
using System;
using AutoMapper;
using FluentValidation;
using Inventory.Application.Common.Abstracts;
using Inventory.Application.DataTransferObjects.ProductDto;
using Inventory.Domain.Entities;
```

### Entity Guidelines

- Use `Guid` for all primary keys
- Include soft delete: `bool IsDeleted { get; set; } = false;`
- Use `DateTime` for timestamps with `CreatedAt`, `UpdatedAt`
- Default property values in property initializer:
```csharp
public Guid Id { get; set; } = Guid.NewGuid();
public string Name { get; set; } = string.Empty;
```

### Error Handling

- Use `KeyNotFoundException` for not found errors:
```csharp
return await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found");
```

- Use FluentValidation for request validation:
```csharp
await validator.ValidateAndThrowAsync(request);
```

- Custom middleware for global exception handling exists at `Inventory.API/Middlewares/ExceptionHandlingMiddleware.cs`

### API Controllers

- Use attribute routing with `[ApiController]`
- Add Swagger annotations: `[HttpGet]`, `[Route("api/[controller]")]`, `[ProducesResponseType]`
- Async methods return `Task<T>`:
```csharp
[HttpGet]
[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
public async Task<ActionResult<ProductResponse>> GetProduct(Guid id)
```

### Database

- Entity Framework Core with Code-First migrations
- Connection string in `appsettings.json`
- Use migrations folder for tracking changes
- Run migrations:
```bash
dotnet ef migrations add [MigrationName] --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```

### Packages Used
- **AutoMapper** (v16.1.1): Object-to-object mapping
- **FluentValidation** (v12.1.1): Request validation
- **Swashbuckle.AspNetCore** (v10.1.7): Swagger/OpenAPI

## Common Tasks

### Adding New Entity
1. Create entity in `Inventory.Domain/Entities/`
2. Add DTOs in `Inventory.Application/DataTransferObjects/[Entity]Dto/`
3. Create repository interface in `Inventory.Application/Common/Abstracts/`
4. Implement repository in `Inventory.Infrastructure/Repositories/`
5. Create service interface and implementation in `Inventory.Application/Services/`
6. Create validator in `Inventory.Application/Common/Validations/`
7. Add AutoMapper profile in `Inventory.Application/Profiles/`
8. Create controller in `Inventory.API/Controllers/`
9. Add DI registration in `Inventory.Application/DependencyInjection.cs`

### Adding a Migration
```bash
dotnet ef migrations add [MigrationName] --project Inventory.Infrastructure --startup-project Inventory.API
```

### Database Reset
```bash
dotnet ef database drop --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef migrations add InitialCreate --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```
