# AGENTS.md - Inventory System Backend

## Architecture

Clean Architecture with 4 projects:
- `Inventory.Domain` - Entities, enums, builders (no external deps)
- `Inventory.Application` - Services, DTOs, validators, interfaces
- `Inventory.Infrastructure` - EF Core, repositories, JWT service
- `Inventory.API` - Controllers, middleware, entry point

Entry point: `Inventory.API/Program.cs`

## Build & Run

```bash
dotnet build
dotnet run --project Inventory.API
```

Database is seeded automatically on startup via `DatabaseSeeder.SeedAsync()`. Requires PostgreSQL at `localhost:5432`.

## Dependencies

- .NET 10.0 (preview)
- EF Core 10.0.5 + Npgsql
- JWT Bearer authentication
- AutoMapper, FluentValidation
- Swashbuckle (Swagger)

## API Access

Swagger UI at `/swagger` in Development mode. Uses Bearer token auth.

## Testing

No test project found in this repo.