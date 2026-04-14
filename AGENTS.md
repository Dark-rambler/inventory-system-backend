# AGENTS.md - Developer Guidelines

## Project Overview

.NET 10 inventory management system with Clean Architecture (4 projects):
- **Inventory.Domain**: Entities
- **Inventory.Application**: Services, DTOs, validators, interfaces
- **Inventory.Infrastructure**: EF Core, repositories, PostgreSQL
- **Inventory.API**: Controllers, entry point

## Commands

```bash
dotnet build
dotnet run --project Inventory.API/Inventory.API.csproj
dotnet ef migrations add [Name] --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database drop --project Inventory.Infrastructure --startup-project Inventory.API
```

## Architecture

- **CQRS-lite**: Services return DTOs, not entities
- **Repository Pattern**: interfaces in `Inventory.Application/Common/Abstracts/`, impl in `Inventory.Infrastructure/Repositories/`
- **Error handling**: Use `KeyNotFoundException` for not found; `await validator.ValidateAndThrowAsync(request)` for validation
- **Soft delete**: Entities include `bool IsDeleted { get; set; } = false`
- **Middleware**: Custom `ExceptionHandlingMiddleware` catches all exceptions and returns ProblemDetails JSON

## Code Patterns

- **Entities**: Primary key `Guid` (default `Guid.NewGuid()`), timestamps `CreatedAt`, `UpdatedAt`
- **DTOs**: Records
- **Services**: Primary constructors (C# 12): `public class ProductService(IProductRepository repo) : IProductService { }`

## Database

- PostgreSQL on localhost:5432, database `inventory`
- Connection: `appsettings.json` -> `ConnectionStrings.DefaultConnection`
- Reset: `dotnet ef database drop` then `dotnet ef database update`

## Runtime

- **Seeding**: `DatabaseSeeder.SeedAsync()` runs on startup in `Program.cs` (automatic on app start)
- **JWT**: Configured in `appsettings.json` (`JwtSettings`) - secret is hardcoded, change in production
- **CORS**: Policy named `"MyCustomCors"` (allows all origins/headers/methods)
- **Swagger**: Available at `/swagger` in development

## Adding Entity

1. `Inventory.Domain/Entities/[Entity].cs`
2. DTOs: `Inventory.Application/DataTransferObjects/[Entity]Dto/`
3. Interface: `Inventory.Application/Common/Abstracts/I[Entity]Repository.cs`
4. Repository impl: `Inventory.Infrastructure/Repositories/[Entity]Repository.cs`
5. Service: `Inventory.Application/Services/[Entity]Service/`
6. Validator: `Inventory.Application/Common/Validations/[Entity]Validator.cs`
7. Mapper: `Inventory.Application/Profiles/[Entity]Profile.cs`
8. Controller: `Inventory.API/Controllers/[Entity]Controller.cs`
9. DI: `Inventory.Application/DependencyInjection.cs` (register service and validator)