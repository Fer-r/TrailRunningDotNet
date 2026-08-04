<RULE[user_project]>

# Developer Profile & Environment

## System
- **OS:** Windows (PowerShell)
- **SDK:** .NET 10.0 SDK (10.0.302)
- **Runtimes Installed:** .NET 10.0.10, .NET 8.0.29, .NET 6.0.36
- **Target Framework:** `net10.0` — Always use this. Never target older frameworks unless explicitly asked.
- **C# Version:** C# 14 (ships with .NET 10)
- **IDE:** Antigravity IDE (VS Code based)
- **Package Manager:** NuGet (via `dotnet add package`)

## Developer Level
- **Status:** Advanced student finishing an EOI .NET course. Has prior full-stack experience.
- **Known Topics (from EOI course):** C# fundamentals, OOP (inheritance, interfaces, records, LINQ, lambdas, collections), file I/O, exception handling, SQLite (raw ADO.NET + EF Core), HTML, ASP.NET Core Razor Pages, Entity Framework Core 10, Dependency Injection, Service Layer pattern.
- **Prior Experience (BeruFoods project):** Built a full-stack food delivery platform with Symfony 7 (PHP), React 18 (Vite), Doctrine ORM, LexikJWT authentication, MySQL, Docker Compose, Mercure real-time, and Railway deployment. Understands REST APIs, JWT auth flows, ORM relationships, service injection, structured error handling, and Docker infrastructure.
- **Attitude:** Ambitious. Migrating a real Symfony backend to .NET 10 as final project. Comfortable with complex architectures.

## Active Project Contexts
1. **RacehubApi (ASP.NET Core Web API + Razor Pages + EF Core 10 + SQLite):** Final course project — migration of a Symfony backend for a Trail Running platform. Consumed by an existing React 18 SPA.
2. **ASP.NET Core Razor Pages + EF Core 10 + SQLite:** Course web exercises (Agenda, Tareas, Pasapalabra).
3. **Console Applications (C#):** Course exercises for fundamentals and OOP.

---

# Language & Code Style

## Language
- **Code:** English — all class names, method names, variable names, properties, enums, and constants in English.
- **Comments:** English.
- **Git commits:** English.
- **Documentation / Memory (for course submission):** Spanish.

## Formatting
- Use **4-space indentation** (no tabs).
- Use **file-scoped namespaces** — always `namespace X;` never `namespace X { }`.
- Use **top-level statements** in `Program.cs` for console apps and web apps.
- Use **expression-bodied members** for single-line methods and properties.
- Use **trailing commas** in multi-line collection initializers and enums.
- Max line length: prefer ~120 characters.
- Braces on their own line (Allman style) for multi-line blocks.

---

# C# 14 / .NET 10 — Required Modern Features

Always use these features when writing new code. Do NOT fall back to older patterns.

## Primary Constructors (C# 12+)
Use for dependency injection and simple data classes:
```csharp
// ✅ CORRECT — Primary constructor
public class GameService(IRepository repo, ILogger<GameService> logger)
{
    public Game GetById(int id) => repo.Find(id);
}

// ❌ WRONG — Classic constructor with manual field assignment
public class GameService
{
    private readonly IRepository _repo;
    public GameService(IRepository repo) { _repo = repo; }
}
```

## `field` Keyword (C# 14)
Use the contextual `field` keyword for property backing fields instead of declaring explicit `_backingField`:
```csharp
// ✅ CORRECT
public string Name
{
    get;
    set => field = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
}

// ❌ WRONG
private string _name;
public string Name
{
    get => _name;
    set => _name = value?.Trim() ?? throw new ArgumentNullException(nameof(value));
}
```

## Collection Expressions (C# 12+)
Use `[...]` syntax for all collection initialization:
```csharp
// ✅ CORRECT
int[] numbers = [1, 2, 3, 4, 5];
List<string> names = ["Alice", "Bob"];
int[] combined = [..first, ..second, 99]; // Spread operator

// ❌ WRONG
int[] numbers = new int[] { 1, 2, 3, 4, 5 };
var names = new List<string> { "Alice", "Bob" };
```

## File-Scoped Namespaces (C# 10+)
```csharp
// ✅ CORRECT
namespace MyGame.Models;

public class Player { }

// ❌ WRONG
namespace MyGame.Models
{
    public class Player { }
}
```

## Records for DTOs / Immutable Data / CQRS
Use `record` or `readonly record struct` for any object representing data transfer, read-only view models, or command/query parameters:
```csharp
// ✅ Positional immutable DTO using record
public record CustomerSummaryDto(Guid Id, string FullName, string Email, int TotalOrders);

// ✅ Command record with positional validation
public record UpdateCustomerCommand(Guid CustomerId, string FullName, string PhoneNumber)
{
    public bool IsValid() => !string.IsNullOrWhiteSpace(FullName) && CustomerId != Guid.Empty;
}

// ✅ High-performance heap-free value representation
public readonly record struct PricePoint(DateTime Timestamp, decimal Value);
```

## Null-Conditional Assignment (C# 14)
```csharp
// ✅ CORRECT
player?.Health = maxHealth;

// ❌ WRONG
if (player is not null) { player.Health = maxHealth; }
```

## Extension Members (C# 14)
Use the new `extension` block syntax instead of classic static extension methods when beneficial:
```csharp
// ✅ CORRECT — New C# 14 extension syntax
public static class CollectionExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty => !source.Any();
    }
}

// ❌ WRONG — Old-style extension method for simple property-like behavior
public static class CollectionExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();
}
```
> Note: Classic `this` extension methods still work and are fine for methods. Use `extension` blocks when you need extension properties, operators, or static extension members.

## Unbound Generics with `nameof` & Lambda Parameter Modifiers (C# 14)
Use `nameof` with open generic types for clean logging/diagnostics, and parameter modifiers in lambdas:
```csharp
// ✅ C# 14: nameof in open generics
logger.LogWarning("Cache miss for repository {RepoType}", nameof(IRepository<>));
string dictionaryTypeName = nameof(Dictionary<,>);

// ✅ Parameter modifiers (ref, in, out, scoped) in lambda expressions
var parser = (string input, out int result) => int.TryParse(input, out result);
```

## Pattern Matching
Use modern pattern matching aggressively:
```csharp
// Property patterns
if (enemy is { Health: <= 0, IsBoss: true }) HandleBossDeath(enemy);

// Switch expressions
var tier = score switch
{
    >= 1000 => "S",
    >= 500  => "A",
    >= 100  => "B",
    _       => "C",
};

// List patterns
if (args is [var command, .. var rest]) ProcessCommand(command, rest);
```

## Other Modern Patterns — Always Prefer
| Modern (✅) | Legacy (❌) |
|---|---|
| `string name = "value";` | `String name = "value";` |
| `int count = 0;` | `Int32 count = 0;` |
| `Product p = new();` | `Product p = new Product();` (when type is obvious) |
| `List<Tarea> tareas = await ...` | `var tareas = await ...` (professors prefer explicit types) |
| `$"Score: {score}"` | `string.Format("Score: {0}", score)` |
| `"""raw string"""` | `@"escaped\nstring"` for multi-line |
| `is null` / `is not null` | `== null` / `!= null` |
| `required` keyword | Constructor-only init for required props |
| `async`/`await` for I/O | `.Result` or `.Wait()` (blocks thread) |

---

# Entity Framework Core 10 — Guidelines

**Version:** EF Core 10.0.10 (via `Microsoft.EntityFrameworkCore.Sqlite` NuGet package)
**Database:** SQLite (`Microsoft.Data.Sqlite` 10.0.10)

## Required Practices
1. **`AsNoTracking()`** for all read-only queries. Use `AsNoTrackingWithIdentityResolution()` when querying complex entity graphs.
2. **DTO Projection with `.Select()`** — Never return full entities to the UI; project to `record` DTOs. This makes the SQL query only `SELECT` the needed columns, not `SELECT *`.
3. **`ExecuteUpdateAsync` / `ExecuteDeleteAsync`** for bulk operations (no loading entities into memory to update/delete them).
4. **`AddDbContext`** (or `AddDbContextPool` for high-concurrency production apps).
5. **Code-First** with migrations (`dotnet ef migrations add`, `dotnet ef database update`).
6. **CancellationToken** — Propagate through ALL async methods with `CancellationToken cancellationToken = default` as the last parameter. Always pass it down to EF queries: `.ToListAsync(cancellationToken)`.
7. **`Async` suffix** — All async methods must end in `Async`: `GetAllAsync()`, `CreateAsync()`, `DeleteAsync()`.
8. **Service Layer Pattern** — Never put EF queries directly in PageModels or Controllers. Use injected services with interfaces (`IXxxService`).
9. **Immutable return types** — Service methods returning collections should return `IReadOnlyList<T>` (not `List<T>`) to signal immutability to consumers.
10. **`required` keyword** — Use on non-nullable entity properties that must be set: `public required string Name { get; set; }`.
11. **Complex Types (`[ComplexType]`) & Native JSON Mapping (`.ToJson()`)** — For value objects without database identity (Address, DateRange), use EF Core Complex Types or `.ToJson()` JSON column mapping instead of extra relational tables.
12. **Server-Side Pagination** — Always paginate potentially large collections using `.Skip()` and `.Take()` at the SQL level.
13. **Compiled Queries (`EF.CompileAsyncQuery`)** — Use for hot-path queries executed thousands of times per minute in high-frequency endpoints.

## Complex Types & JSON Mapping Example
```csharp
[ComplexType]
public record Address(string Street, string City, string PostalCode, string Country);

public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Address BillingAddress { get; set; }
}

// Fluent API configuration in DbContext
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>()
        .OwnsOne(c => c.BillingAddress, b => b.ToJson()); // Stores as native JSON column
}
```

## Server-Side Pagination Example
```csharp
public async Task<PagedResult<CustomerSummaryDto>> GetCustomersPagedAsync(
    int pageIndex, 
    int pageSize, 
    CancellationToken ct = default)
{
    var query = db.Customers.AsNoTracking().Where(c => c.IsActive);
    
    int totalCount = await query.CountAsync(ct);
    List<CustomerSummaryDto> items = await query
        .OrderBy(c => c.Name)
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new CustomerSummaryDto(c.Id, c.Name, c.Email, c.Orders.Count))
        .ToListAsync(ct);

    return new PagedResult<CustomerSummaryDto>(items, totalCount, pageIndex, pageSize);
}
```

## EF Core Anti-Patterns to Flag
```csharp
// ❌ Loading all entities then filtering in memory
List<Product> result = context.Products.ToList().Where(p => p.Price > 10).ToList();

// ✅ Filter in the database
List<Product> result = await context.Products.Where(p => p.Price > 10).ToListAsync(ct);

// ❌ N+1 query (loading related data in a loop)
foreach (Order order in orders)
    order.Items = context.Items.Where(i => i.OrderId == order.Id).ToList();

// ✅ Eager loading
List<Order> orders = await context.Orders.Include(o => o.Items).ToListAsync(ct);

// ❌ Over-fetching (returning full entity with 20 columns for a dropdown)
List<Race> races = await context.TrailRunnings.ToListAsync(ct);

// ✅ Project to DTO (SQL only selects needed columns)
IReadOnlyList<RaceDto> races = await context.TrailRunnings
    .AsNoTracking()
    .Select(r => new RaceDto(r.Id, r.Name, r.Date, r.Location))
    .ToListAsync(ct);

// ❌ Loading entities to delete them
List<Participant> participants = await context.Participants.Where(p => p.RaceId == id).ToListAsync(ct);
context.RemoveRange(participants);
await context.SaveChangesAsync(ct);

// ✅ Bulk delete directly in database (1 SQL query)
await context.Participants.Where(p => p.RaceId == id).ExecuteDeleteAsync(ct);
```

---

# ASP.NET Core Razor Pages — Guidelines

1. **Post-Redirect-Get (PRG):** All `OnPostAsync` handlers must return `RedirectToPage()` after mutations. Never return `Page()` after a successful POST.
2. **TempData** for flash notifications after redirects: `TempData["Success"] = "Registered successfully.";`
3. **Tag Helpers** (`asp-for`, `asp-page`, `asp-items`, `asp-validation-for`) — never use raw HTML helpers (`@Html.TextBoxFor`). Tag Helpers produce clean HTML.
4. **DataAnnotations on input records** (`[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`) for model validation. Use `record` with `{ get; init; }` for input models.
5. **PageModel** classes only orchestrate HTTP — delegate business logic to services. Keep `OnGetAsync`/`OnPostAsync` thin.
6. **`ModelState.IsValid`** — Always check before processing in `OnPostAsync`. Return `Page()` if invalid.
7. **`[BindProperty]`** on input models — never bind raw entity classes directly to the form.
8. **View Components (`ViewComponent`)** over Partials with `ViewData` — when building complex reusable UI components (header cart, notifications, dynamic nav), create a strongly-typed `ViewComponent` with its own async data fetching rather than passing untyped `ViewData` to partials.

---

# General Architecture & Error Handling (from Agents.md)

## 1. Feature Folders Organization
Organize pages and components by domain feature folders rather than flat directories:
```text
Pages/
 ├── Shared/
 │    ├── _Layout.cshtml
 │    └── _ValidationScriptsPartial.cshtml
 ├── Customers/
 │    ├── Index.cshtml + Index.cshtml.cs
 │    └── Create.cshtml + Create.cshtml.cs
 └── Orders/
      └── Checkout.cshtml + Checkout.cshtml.cs
```

## 2. Global Exception Handling (`IExceptionHandler`) & `Result<T>`
Do not wrap Razor PageModel handlers or Controllers in monolithic try/catch blocks for infrastructure errors.
- Implement `IExceptionHandler` (.NET 8/10) and register it in `Program.cs` for unhandled infrastructure exceptions.
- Use the **`Result<T>` pattern** (or explicit success/error result objects) for domain business logic outcomes.

## 3. Structured Logging with Source Generators (`[LoggerMessage]`)
For high-frequency or critical service operations, use `[LoggerMessage]` source generator methods to eliminate runtime string formatting overhead:
```csharp
public static partial class LogMessages
{
    [LoggerMessage(
        EventId = 1001, 
        Level = LogLevel.Information, 
        Message = "User {UserId} completed order {OrderId} for {Amount:C}")]
    public static partial void LogOrderCompleted(this ILogger logger, Guid userId, Guid orderId, decimal amount);
}
```

---

# ASP.NET Core Web API — Guidelines

1. **`[ApiController]`** on all controllers — enables automatic model validation and `ProblemDetails` responses.
2. **JWT Bearer Authentication** — Use `Microsoft.AspNetCore.Authentication.JwtBearer` for stateless auth.
3. **BCrypt.Net-Next** for password hashing — never store plain text or MD5/SHA hashes.
4. **CORS** must be configured to allow the React dev server (`http://localhost:5173`).
5. **`[JsonPropertyName]`** on all DTO properties — the React frontend expects a mix of snake_case and camelCase.
6. **Swagger/OpenAPI** enabled in development for interactive API documentation.
7. **Razor Pages** only for the `/register` page (SSR form). All other endpoints are pure JSON API.
8. **No Repository Pattern** — inject `DbContext` directly into service classes (following course convention).

---

# Common Anti-Patterns — Actively Correct These

If the developer writes any of these patterns, flag them and suggest the modern alternative:

| # | Anti-Pattern (❌) | Modern Alternative (✅) | Why |
|---|---|---|---|
| 1 | `namespace X { }` (block-scoped) | `namespace X;` (file-scoped) | Reduces nesting, standard since C# 10 |
| 2 | Classic constructor + `_field` for DI | Primary constructor | Less boilerplate, C# 12+ standard |
| 3 | `new List<int> { 1, 2, 3 }` | `[1, 2, 3]` | Collection expressions, C# 12+ |
| 4 | `new int[] { }` | `[]` | Same — collection expression |
| 5 | `== null` | `is null` | Pattern matching, avoids operator overload issues |
| 6 | `!= null` | `is not null` | Same reason |
| 7 | `.Result` or `.Wait()` on async | `await` | Blocks thread, risk of deadlock |
| 8 | `string.Format()` | `$""` interpolation | Cleaner, compile-time checked |
| 9 | `String`, `Int32`, `Boolean` | `string`, `int`, `bool` | Language keywords preferred per MS guidelines |
| 10 | Manual backing field `_name` | `field` keyword (C# 14) | Less boilerplate when custom accessor logic needed |
| 11 | `if (x != null) x.Prop = val;` | `x?.Prop = val;` | Null-conditional assignment, C# 14 |
| 12 | Classic static extension method for properties | `extension` block | C# 14 extension members |
| 13 | `context.Products.ToList().Where(...)` | `context.Products.Where(...).ToListAsync()` | Filter in DB, not in memory |
| 14 | EF queries in PageModel | Service layer with interface | Separation of concerns |

---

# Project File Templates

## Console App (.csproj)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

## ASP.NET Core Razor Pages (.csproj)
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.10" />
  </ItemGroup>
</Project>
```

---

# Course Context (EOI Programación .NET)

## Modules Completed
1. **Programación Básica:** Types, arrays, operators, control flow, functions, file I/O, error handling. Projects: Laberinto, TresEnRaya.
2. **POO:** Classes, constructors, properties, records, static, inheritance, interfaces, collections (List, Dictionary, HashSet, SortedList), lambdas, LINQ. Projects: Cuatro en Raya, Invasores del Espacio, Juego Naves (Raylib-cs).
3. **Acceso a Datos:** Relational DB theory, raw SQLite access with `Microsoft.Data.Sqlite`, parameterized queries. Projects: Agenda, Ahorcado, Pasapalabra, Lista de Tareas.
4. **HTML:** Full 10-unit web foundations (structure, forms, media, tables, semantic HTML, Bootstrap).
5. **Razor Pages:** ASP.NET Core server-rendered pages, PageModel, Tag Helpers, PRG pattern. Projects: Agenda web, Pasapalabra web, Lista de Tareas web.
6. **Entity Framework Core:** EF Core 10 Code-First, migrations, relationships (1:N, N:M), DI, service layer, DTOs, query optimization. Projects: Alumnos, Equipos, Tareas.

## Final Project
- **Budget:** ~12.5 hours
- **Concept:** RacehubApi — Migration of a Symfony backend to .NET 10 Web API for a Trail Running platform
- **Frontend:** Existing React 18 SPA (not modified, consumed as-is)
- **Must demonstrate:** OOP, EF Core persistence, JWT authentication, service layer, xUnit testing, Razor Pages (register)
- **Deliverable includes:** A written memory/report (in Spanish) following course PDF examples

---

# IA Agent Checklist (from professors' Agents.md)

Before emitting or modifying any code file, the agent MUST verify:

- [ ] **C# 14 syntax?** Primary constructors, `field` keyword, collection expressions `[]`, `record` for DTOs, file-scoped namespaces.
- [ ] **Async parameters correct?** Every `async` method passes `CancellationToken ct = default` as last parameter, propagated to ORM/HTTP calls.
- [ ] **EF Core reads optimized?** `.AsNoTracking()`, project with `.Select()` to DTO records, no full-table loads.
- [ ] **Bulk mutations efficient?** Evaluated `ExecuteUpdateAsync`/`ExecuteDeleteAsync` instead of load-modify-save loops.
- [ ] **PageModel thin and safe?** Uses PRG (`RedirectToPage()` after POST), validates `ModelState`, doesn't bind raw entities.
- [ ] **HTML clean?** Uses Tag Helpers (`asp-for`, `asp-validation-for`) instead of legacy HTML Helpers.
- [ ] **Explicit types?** Uses `List<Race> races = ...` instead of `var races = ...` (professors' convention).
- [ ] **Immutable returns?** Service methods return `IReadOnlyList<T>` for collections, `record` for DTOs.
- [ ] **No password exposure?** DTOs never contain `Password` or hash fields. Only entities have them.
- [ ] **JSON contract exact?** All DTO properties have `[JsonPropertyName]` matching the React frontend expectations.

</RULE[user_project]>
