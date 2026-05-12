# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build Inventory.sln

# Run the API (development)
dotnet run --project Inventory.API

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~ProviderServiceTests"

# Add a new migration
dotnet ef migrations add <MigrationName> --project Inventory.Infrastructure --startup-project Inventory.API

# Apply migrations
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```

## Architecture

This is a .NET 10 REST API using Clean Architecture with four layers:

- **Inventory.Domain** — Pure entities with no external dependencies. Complex entities use the Builder pattern.
- **Inventory.Application** — Business logic services, DTOs, AutoMapper profiles, FluentValidation validators, and repository interfaces (`Common/Abstracts/`).
- **Inventory.Infrastructure** — EF Core DbContext, repository implementations, `JwtService`, `ExcelReader` for bulk imports, and database seeding.
- **Inventory.API** — Controllers, global `ExceptionHandlingMiddleware`, and `Program.cs` wiring.

### Request flow

`Controller → Service → Repository → InventoryDbContext (PostgreSQL)`

### Dependency Injection

Each non-API layer exposes a static `DependencyInjection` extension class that registers its own services. `Program.cs` calls all of them. New services, repositories, validators, and AutoMapper profiles must be registered there.

### Repository pattern

All data access goes through interfaces defined in `Inventory.Application/Common/Abstracts/`. Implementations live in `Inventory.Infrastructure/Repositories/`. Repositories handle pagination, filtering, and soft deletes (`IsDeleted` query filter applied globally in the DbContext).

### Exception handling

`ExceptionHandlingMiddleware` maps exceptions to HTTP status codes:
- `ValidationException` → 400
- `KeyNotFoundException` → 404
- `UnauthorizedAccessException` → 401
- `ArgumentException` / `InvalidOperationException` → 400
- Everything else → 500

Throw these exception types from services; do not return error status codes directly.

## Key patterns

- **IDs**: `Category` and `Product` use `int` (auto-increment). Most other entities use `Guid`.
- **Soft deletes**: Set `IsDeleted = true`; never hard-delete via the normal service layer.
- **Pagination**: Use `PaginatedList<T>` for list endpoints.
- **Inventory movements**: Resolved via `MovementStrategyResolver` (Strategy pattern) — add new movement types by implementing the strategy interface.
- **Bulk upload**: Products can be imported via Excel using `ExcelReader` (ClosedXML).
- **Audit history**: User actions are recorded in `AuditHistory`; wire new write operations into this flow.

## Database

PostgreSQL via Npgsql EF Core provider. Default dev connection string is in `appsettings.json` (`Host=localhost;Port=5432;Database=inventory;Username=postgres;Password=mysecretpassword`).

## Authentication

JWT Bearer tokens. `JwtService` (Infrastructure) generates tokens with user ID, username, role, and email claims. Use `[Authorize]` on protected endpoints and `[Authorize(Roles = "Admin")]` for admin-only routes. The login endpoint in `AuthService` is the only unauthenticated write path.

## Tests

xUnit with Moq. Tests live in `Inventory.Tests/`. Follow the existing pattern: mock `IRepository`, `IMapper`, and `IValidator` dependencies; test both the happy path and `KeyNotFoundException` scenarios for each CRUD operation.
