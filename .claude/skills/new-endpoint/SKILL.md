---
name: new-endpoint
description: 'Step-by-step guide to create a new CRUD endpoint following the Clean Architecture pattern of this project.'
---

# Create a New Endpoint

Your task is to scaffold all files needed for a new endpoint in this project. The user will tell you the entity name and optionally the fields. Follow every step below in order. Use the Category entity as the canonical reference for naming, structure, and DI registration.

---

## Step 0 — Gather requirements

Ask the user for:
1. **Entity name** (e.g., `Supplier`)
2. **Fields** (name, type, required/optional)
3. **ID type** — `int` (auto-increment, like Category/Product) or `Guid` (like most other entities)
4. **Role restrictions** — which HTTP verbs require `[Authorize(Roles = "Admin")]`

---

## Step 1 — Domain entity (`Inventory.Domain/Entities/<Entity>.cs`)

```csharp
public class <Entity>
{
    public <IdType> Id { get; set; }
    // user-defined fields …
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
}
```

Rules:
- No external dependencies — pure POCO.
- Always include `IsDeleted`, `CreatedAt`, `UpdatedAt`.
- If `IdType` is `Guid`, initialize `Id = Guid.NewGuid()`.
- Add navigation properties only when a real relationship exists.

---

## Step 2 — DTOs (`Inventory.Application/DataTransferObjects/<Entity>Dto/`)

Create three files:

**`<Entity>Request.cs`** — input fields only (no Id, no audit fields).

**`<Entity>Response.cs`** — all fields the caller should see, including `Id`, `CreatedAt`, `UpdatedAt`.

**`<Entity>SearchParams.cs`**

```csharp
public class <Entity>SearchParams
{
    // optional filter fields (string?, etc.)
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

---

## Step 3 — FluentValidation validator (`Inventory.Application/Common/Validations/<Entity>RequestValidation.cs`)

```csharp
public class <Entity>RequestValidation : AbstractValidator<<Entity>Request>
{
    public <Entity>RequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        // add rules for every required or constrained field
    }
}
```

---

## Step 4 — AutoMapper profile (`Inventory.Application/Profiles/<Entity>Profile.cs`)

```csharp
public class <Entity>Profile : Profile
{
    public <Entity>Profile()
    {
        CreateMap<<Entity>Request, <Entity>>();
        CreateMap<<Entity>, <Entity>Response>();
    }
}
```

---

## Step 5 — Repository interface (`Inventory.Application/Common/Abstracts/I<Entity>Repository.cs`)

```csharp
public interface I<Entity>Repository
{
    Task<PaginatedList<<Entity>>> Get<Entity>sAsync(/* filter params */, int page, int pageSize);
    Task<<Entity>?> Get<Entity>ByIdAsync(<IdType> id);
    Task<<Entity>> Create<Entity>Async(<Entity> entity);
    Task Update<Entity>Async(<Entity> entity);
    Task Delete<Entity>Async(<Entity> entity);
}
```

---

## Step 6 — Service interface + implementation (`Inventory.Application/Services/<Entity>Service/`)

**`I<Entity>Service.cs`**

```csharp
public interface I<Entity>Service
{
    Task<PaginatedList<<Entity>Response>> Get<Entity>sAsync(<Entity>SearchParams searchParams);
    Task<<Entity>Response> Get<Entity>ByIdAsync(<IdType> id);
    Task<<Entity>Response> Create<Entity>Async(<Entity>Request request);
    Task Update<Entity>Async(<IdType> id, <Entity>Request request);
    Task Delete<Entity>Async(<IdType> id);
}
```

**`<Entity>Service.cs`**

```csharp
public class <Entity>Service(
    I<Entity>Repository repository,
    IMapper mapper,
    IValidator<<Entity>Request> validator) : I<Entity>Service
{
    public async Task<PaginatedList<<Entity>Response>> Get<Entity>sAsync(<Entity>SearchParams searchParams)
    {
        var items = await repository.Get<Entity>sAsync(/* params from searchParams */, searchParams.Page, searchParams.PageSize);
        return new PaginatedList<<Entity>Response>(
            mapper.Map<List<<Entity>Response>>(items.Items),
            items.TotalCount, items.PageIndex, items.PageSize);
    }

    public async Task<<Entity>Response> Get<Entity>ByIdAsync(<IdType> id) =>
        mapper.Map<<Entity>Response>(await Find<Entity>ById(id));

    public async Task<<Entity>Response> Create<Entity>Async(<Entity>Request request)
    {
        await validator.ValidateAndThrowAsync(request);
        return mapper.Map<<Entity>Response>(
            await repository.Create<Entity>Async(mapper.Map<<Entity>>(request)));
    }

    public async Task Update<Entity>Async(<IdType> id, <Entity>Request request)
    {
        await validator.ValidateAndThrowAsync(request);
        await repository.Update<Entity>Async(mapper.Map(request, await Find<Entity>ById(id)));
    }

    public async Task Delete<Entity>Async(<IdType> id) =>
        await repository.Delete<Entity>Async(await Find<Entity>ById(id));

    private async Task<<Entity>> Find<Entity>ById(<IdType> id) =>
        await repository.Get<Entity>ByIdAsync(id)
            ?? throw new KeyNotFoundException($"<Entity> with id {id} doesn't exist");
}
```

---

## Step 7 — DbContext (`Inventory.Infrastructure/Context/InventoryDbContext.cs`)

Add the DbSet and Fluent API config inside `OnModelCreating`:

```csharp
public DbSet<<Entity>> <Entity>s { get; set; }
```

Inside `OnModelCreating`:

```csharp
// For int ID:
modelBuilder.Entity<<Entity>>()
    .Property(e => e.Id)
    .UseIdentityByDefaultColumn();

// For Guid ID — no extra config needed; EF handles it.

modelBuilder.Entity<<Entity>>()
    .HasQueryFilter(e => !e.IsDeleted);
```

---

## Step 8 — Filter extension (`Inventory.Infrastructure/Extensions/IQuerableExtensions.cs`)

Add a new extension block for the entity:

```csharp
extension(IQueryable<<Entity>> source)
{
    public IQueryable<<Entity>> Filters<Entity>(string? name /* + any other filter params */)
    {
        if (!string.IsNullOrEmpty(name))
            source = source.Where(e => e.Name.ToLower().Contains(name.ToLower()));
        return source;
    }
}
```

---

## Step 9 — Repository implementation (`Inventory.Infrastructure/Repositories/<Entity>Repository.cs`)

```csharp
public class <Entity>Repository(InventoryDbContext context) : I<Entity>Repository
{
    public async Task<PaginatedList<<Entity>>> Get<Entity>sAsync(/* filter params */, int page, int pageSize) =>
        await context.<Entity>s
            .AsQueryable()
            .OrderByDescending(e => e.CreatedAt)
            .Filters<Entity>(/* filter params */)
            .ToPaginatedListAsync(page, pageSize);

    public Task<<Entity>?> Get<Entity>ByIdAsync(<IdType> id) =>
        context.<Entity>s.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<<Entity>> Create<Entity>Async(<Entity> entity)
    {
        context.<Entity>s.Add(entity);
        await context.SaveChangesAsync();
        return await context.<Entity>s.FirstAsync(e => e.Id == entity.Id);
    }

    public async Task Update<Entity>Async(<Entity> entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        context.<Entity>s.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete<Entity>Async(<Entity> entity)
    {
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }
}
```

---

## Step 10 — Controller (`Inventory.API/Controllers/<Entity>Controller.cs`)

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class <Entity>Controller(I<Entity>Service service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedList<<Entity>Response>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get<Entity>sAsync([FromQuery] <Entity>SearchParams searchParams) =>
        Ok(await service.Get<Entity>sAsync(searchParams));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(<Entity>Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get<Entity>ByIdAsync(<IdType> id) =>
        Ok(await service.Get<Entity>ByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(<Entity>Response), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create<Entity>Async([FromBody] <Entity>Request request) =>
        Ok(await service.Create<Entity>Async(request));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update<Entity>Async(<IdType> id, [FromBody] <Entity>Request request)
    {
        await service.Update<Entity>Async(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete<Entity>Async(<IdType> id)
    {
        await service.Delete<Entity>Async(id);
        return NoContent();
    }
}
```

---

## Step 11 — Register in DI

**`Inventory.Application/DependencyInjection.cs`** — add inside `AddApplication()`:

```csharp
services.AddAutoMapper(cfg => { }, /* existing profiles */, typeof(<Entity>Profile));
services.AddScoped<IValidator<<Entity>Request>, <Entity>RequestValidation>();
services.AddScoped<I<Entity>Service, <Entity>Service>();
```

**`Inventory.Infrastructure/DependencyInjection.cs`** — add inside `AddInfrastructure()`:

```csharp
services.AddScoped<I<Entity>Repository, <Entity>Repository>();
```

---

## Step 12 — EF Core migration

After all files are in place, run:

```bash
dotnet ef migrations add Add<Entity> --project Inventory.Infrastructure --startup-project Inventory.API
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.API
```

---

## Step 13 — Tests (`Inventory.Tests/<Entity>ServiceTests.cs`)

Write xUnit tests with Moq following the existing pattern:

- Mock `I<Entity>Repository`, `IMapper`, `IValidator<<Entity>Request>`.
- Test **happy path** and **`KeyNotFoundException`** for every method.
- Follow the AAA pattern (Arrange / Act / Assert).

---

## Checklist

- [ ] Domain entity with soft-delete + audit fields
- [ ] Request / Response / SearchParams DTOs
- [ ] FluentValidation validator
- [ ] AutoMapper profile
- [ ] Repository interface
- [ ] Service interface + implementation
- [ ] DbContext `DbSet` + `HasQueryFilter`
- [ ] Filter extension method
- [ ] Repository implementation
- [ ] Controller with correct `[Authorize]` attributes
- [ ] DI registration in Application and Infrastructure layers
- [ ] EF migration applied
- [ ] Unit tests
