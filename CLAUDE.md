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

- **Inventory.Domain** — Pure entities with no external dependencies. All entities use the Builder pattern (`Inventory.Domain/Entities/Builders/`). Domain enums live in `Inventory.Domain/Enum/`: `EnumMovementType` (Entry, Exit, Transfer), `EnumEntity` (InventoryMovement, Sale, Purchase), `EnumAction` (Create).
- **Inventory.Application** — Business logic services, DTOs (`DataTransferObjects/*Dto/`), AutoMapper profiles, FluentValidation validators, and repository interfaces (`Common/Abstracts/`).
- **Inventory.Infrastructure** — EF Core DbContext, repository implementations, `JwtService`, `ExcelReader` for bulk imports, and database seeding.
- **Inventory.API** — Controllers, global `ExceptionHandlingMiddleware`, and `Program.cs` wiring.

### Request flow

`Controller → Service → Repository → InventoryDbContext (PostgreSQL)`

### Dependency Injection

Each non-API layer exposes a static `DependencyInjection` extension class that registers its own services. `Program.cs` calls all of them. Registration patterns:
- Services/Repositories: `services.AddScoped<IFoo, Foo>()`
- Validators: `services.AddScoped<IValidator<FooRequest>, FooRequestValidation>()` in `Inventory.Application/DependencyInjection.cs`
- AutoMapper profiles: add `typeof(FooProfile)` to the `AddAutoMapper(cfg => {}, typeof(...), ...)` call in `Inventory.Application/DependencyInjection.cs`
- Movement strategies: `services.AddScoped<IInventoryMovementStrategy, FooStrategy>()` in `Inventory.Infrastructure/DependencyInjection.cs`

### Repository pattern

All data access goes through interfaces defined in `Inventory.Application/Common/Abstracts/`. Implementations live in `Inventory.Infrastructure/Repositories/`. Repositories always filter by `businessId` for tenant isolation. Complex filtering logic (name search, date ranges, etc.) belongs in `Inventory.Infrastructure/Extensions/IQuerableExtensions.cs` as extension methods — keep repositories thin.

### Exception handling

`ExceptionHandlingMiddleware` maps exceptions to HTTP status codes:
- `ValidationException` → 400 (field errors surfaced in `extensions["errors"]`)
- `KeyNotFoundException` → 404
- `UnauthorizedAccessException` → 401
- `ArgumentException` / `InvalidOperationException` → 400
- Everything else → 500

Throw these exception types from services; do not return error status codes directly.

## Key patterns

### IDs

- `Category`, `Product`, `Measure`, `Role`: `int` with auto-increment (`.UseIdentityByDefaultColumn()`)
- Everything else: `Guid` with `uuid_generate_v4()` default

### Multi-tenancy

Every major entity has a `BusinessId: Guid` foreign key. Controllers receive it via `[FromHeader][BindRequired] Guid businessId` and pass it to every service call. Every repository query must filter by `businessId` — failing to do so leaks cross-tenant data.

### Soft deletes

Set `IsDeleted = true`; never hard-delete. The DbContext applies `HasQueryFilter(e => !e.IsDeleted)` globally on all soft-deletable entities, so deleted records are invisible to all queries automatically.

### Pagination

Use `PaginatedList<T>` for list endpoints (`Inventory.Application/Common/Pagination/`). Create via the `ToPaginatedListAsync(pageIndex, pageSize)` extension in `IQuerableExtensions`. GET endpoints accept a `*SearchParams` DTO via `[FromQuery]` containing `pageIndex`, `pageSize`, and optional filters.

### Inventory movements

Resolved via `MovementStrategyResolver` (Strategy pattern in `Inventory.Application/Services/InventoryMovementService/InventoryMovementStrategy/`). Add new movement types by:
1. Implementing `IInventoryMovementStrategy` (set `Type` property to the new `EnumMovementType` value)
2. Adding the new enum value to `EnumMovementType`
3. Registering the strategy as scoped in `Inventory.Infrastructure/DependencyInjection.cs`

### Stock management

`BranchProduct` and `WarehouseProduct` are junction tables with composite PKs (`{BranchId/WarehouseId, ProductId}`). They expose `AddStock(int)` and `ReduceStock(int)` domain methods that validate quantities — always use these methods instead of assigning the `Stock` property directly.

### Builder pattern

All entities must be constructed via their builder in `Inventory.Domain/Entities/Builders/` — never use `new Entity { ... }` directly. Builders exist for: `Product`, `Category`, `BranchProduct`, `WarehouseProduct`, `InventoryMovement`, `Purchase`, `PurchaseDetail`, `Sale`, `SaleDetail`, `AuditHistory`, and `RefreshToken`. When adding a new entity, create a corresponding builder in the same directory.

### Audit history

Create via `AuditHistoryBuilder` (never construct `AuditHistory` directly). Wire new write operations into `AuditHistoryService`. When auditing a new entity type, add it to `EnumEntity` in the Domain layer (`Inventory.Domain/Enum/EnumEntity.cs`).

### Testable time

Use `IDateTimeProvider` (registered as singleton) wherever `DateTime.UtcNow` is needed. Do not call `DateTime.UtcNow` directly in services or repositories.

### Bulk upload

Products can be imported via Excel using `ExcelReader` (ClosedXML). The template endpoint `GET /api/products/template` returns the expected .xlsx format.

## Database

PostgreSQL via Npgsql EF Core provider. Default dev connection string is in `appsettings.json` (`Host=localhost;Port=5432;Database=inventory;Username=postgres;Password=mysecretpassword`). Warehouse and Branch each have a one-to-one relationship with `Location`.

`DatabaseSeeder.SeedAsync()` runs on every startup (called in `Program.cs`, idempotent). It seeds 1 Business, 2 Roles, 6 Measures, 4 Categories, 6 Products, 2 Warehouses, 2 Branches, and 2 Users (admin/manager with BCrypt-hashed passwords). Add new seed data to `Inventory.Infrastructure/Context/DatabaseSeeder.cs`.

## Authentication

JWT Bearer tokens. `JwtService` (Infrastructure) generates tokens with user ID, username, role, email, businessId, and businessName claims. Use `[Authorize]` on protected endpoints and `[Authorize(Roles = "Admin")]` for admin-only routes. Only `POST /api/auth/login` and `POST /api/auth/refresh` are unauthenticated.

**Refresh tokens** — `AuthService.RefreshTokenAsync()` validates an incoming refresh token, revokes it, and issues a new JWT + refresh token pair (7-day rotation). The `RefreshToken` entity has `IsRevoked` and an expiry timestamp; store it via `IRefreshTokenRepository`. Auth DTOs (`LoginRequest`, `RefreshTokenRequest`, `LoginResponse`) are C# `record` types.

`IPasswordHasher` (BCrypt.Net) is injected into `AuthService` (login verification) and `UserService` (new user creation). Never hash passwords manually.

`ICurrentUserService` extracts the acting user's `UserId` (Guid) from JWT claims via `IHttpContextAccessor`. Inject this wherever a service needs the current user — do not access `IHttpContextAccessor` directly in services.

`IBusinessContextService` (Infrastructure) extracts `businessId` from the request header, throwing `UnauthorizedAccessException` if the header is absent and `ArgumentException` if it is not a valid GUID.

In production, CORS allowed origins are read from `Cors:AllowedOrigins` in configuration.

## Tests

xUnit with Moq. Tests live in `Inventory.Tests/`. Follow the existing pattern: mock `IRepository`, `IMapper`, and `IValidator` dependencies; test both the happy path and `KeyNotFoundException` scenarios for each CRUD operation. Test naming convention: `MethodName_Condition_ExpectedResult`. Use private helper methods (`CreateEntity()`, `CreateRequest()`) for test data setup.
