# AGENTS.md - Developer Guidelines

## Project Overview

.NET 10 inventory management system using Clean Architecture with 4 projects:
- **Inventory.Domain**: Entities and domain models
- **Inventory.Application**: Services, DTOs, validators, interfaces
- **Inventory.Infrastructure**: EF Core, repositories, PostgreSQL database context
- **Inventory.API**: Controllers, middleware, entry point (port 5000)

## Build & Development Commands

```bash
dotnet build
dotnet run --project Inventory.API/Inventory.API.csproj
```

## Architecture

- **Clean Architecture**: Domain -> Application -> Infrastructure -> API
- **Repository Pattern**: interfaces in `Inventory.Application/Common/Abstracts/`, implementations in `Inventory.Infrastructure/Repositories/`
- **CQRS-lite**: Services return DTOs (not entities)

### Project Structure
```
Inventory.Application/
├── Common/Abstracts/       # I[Entity]Repository interfaces
├── Common/Validations/     # FluentValidation validators
├── DataTransferObjects/[Entity]Dto/  # Request, Response, SearchParams
├── Services/[Entity]Service/         # IEntityService, EntityService
├── Profiles/               # AutoMapper profiles
└── DependencyInjection.cs  # DI registration

Inventory.Infrastructure/
├── Context/       # InventoryDbContext (EF Core)
├── Repositories/  # Repository implementations
└── Migrations/    # EF Core migrations
```

## Code Style

### Entity Guidelines
- Primary keys: `Guid` (use `Guid.NewGuid()` as default)
- Include soft delete: `bool IsDeleted { get; set; } = false;`
- Timestamps: `CreatedAt`, `UpdatedAt` as `DateTime`

### C# Patterns
- **Primary constructors** (C# 12+): `public class ProductService(IProductRepository repo) : IProductService { }`
- **Records for DTOs**: `public record ProductResponse(Guid Id, string Name);`

### Error Handling
- Not found: `throw new KeyNotFoundException($"Entity with id {id} not found")`
- Validation: `await validator.ValidateAndThrowAsync(request);`
- Global handling: `Inventory.API/Middlewares/ExceptionHandlingMiddleware.cs`

## Database

PostgreSQL is used. Connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=inventory;Username=postgres;Password=mysecretpassword"
}
```

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

## Important Runtime Behavior

- **Seeding**: Database is seeded on startup via `DatabaseSeeder.SeedAsync()` in `Program.cs`
- **JWT Auth**: Configured in `appsettings.json` (`JwtSettings`), requires `Authentication` middleware
- **CORS**: Enabled with `"MyCustomCors"` policy (allows all origins/headers/methods)

## Key Packages

| Package | Version | Purpose |
|---------|---------|---------|
| AutoMapper | 16.1.1 | DTO mapping |
| FluentValidation | 12.1.1 | Request validation |
| Swashbuckle.AspNetCore | 10.1.7 | Swagger/OpenAPI |
| BCrypt.Net-Next | 4.1.0 | Password hashing |
| Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.1 | PostgreSQL EF provider |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.5 | JWT auth |

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
