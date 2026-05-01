# AGENTS.md

## Build & Run
- Build: `dotnet build`
- Run API: `dotnet run --project Inventory.API`
- Run tests: `dotnet test`

## Architecture
- **Domain**: Entities and business rules
- **Application**: Services, DTOs, validators (depends on Domain)
- **Infrastructure**: EF Core, PostgreSQL, JWT auth (depends on Application + Domain)
- **API**: Controllers, middleware (depends on Application + Infrastructure)

## Database
- PostgreSQL at `localhost:5432` (configured in `appsettings.json`)
- Credentials: `postgres` / `mysecretpassword`
- Auto-seeded on startup via `DatabaseSeeder.SeedAsync`

## Tech Stack
- .NET 10.0
- Entity Framework Core 10.0.7 with Npgsql
- xUnit + Moq for testing
- JWT Bearer authentication
- FluentValidation for validation
- Swagger available at `/swagger` in development

## Testing
- Tests in `Inventory.Tests/` using xUnit
- Run specific test: `dotnet test --filter "FullyQualifiedName~ProviderServiceTests"`