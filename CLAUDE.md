# Conway's Game of Life — Claude Instructions

## Project Overview

C# (.NET 8.0) implementation of Conway's Game of Life with three components:
- **Conway.Domain** — core game logic (library)
- **Conway.UI.Console** — terminal UI using Spectre.Console
- **Conway.UI.Api** — REST API using ASP.NET Core

## Build & Test

```bash
# Build
dotnet build

# Run tests (48 tests with xUnit)
dotnet test

# Run console app
dotnet run --project Conway.UI.Console

# Run API (http://localhost:5151, Swagger at /swagger)
dotnet run --project Conway.UI.Api
```

## Architecture

- **Dependency injection** via `IServiceCollection`; `IGameGrid` registered as singleton
- **`IGameGrid`** is the primary domain interface — prefer it over `GameGrid` concretely
- **`GameGrid`** uses `ConcurrentDictionary` + `SemaphoreSlim` for thread safety
- **`World`** contains the grid state and Conway's rule logic
- **`GameStatus`** is a record (immutable) DTO used to snapshot state
- API controllers return proper HTTP status codes; domain returns `bool` results
- Console UI follows renderer abstraction: `IGameRenderer` / `SpectreLiveRenderer`

## Conventions

- Namespace pattern: `Conway.<Layer>` (e.g., `Conway.Domain`, `Conway.UI.Console`, `Conway.API`)
- PascalCase for all class/file names
- Tests use xUnit + AutoFixture + Moq, AAA pattern
- UI strings (menus, prompts) are in Spanish; domain model is in English
- Grid dimensions constrained to 10–100 in the console UI
- Sealed classes for `World` and `Cell`; record type for `GameStatus`

## Key Files

| File | Purpose |
|------|---------|
| `Conway.Domain/IGameGrid.cs` | Primary domain interface |
| `Conway.Domain/GameGrid.cs` | Thread-safe game manager |
| `Conway.Domain/World.cs` | Game state + Conway rules |
| `Conway.UI.Api/Controllers/GameController.cs` | REST endpoints |
| `Conway.UI.Console/Program.cs` | Console entry point & menu |
| `Conway.UI.Console/Rendering/SpectreLiveRenderer.cs` | Live Spectre.Console renderer |
| `Conway.Tests/` | All tests |

## Active Branch

Currently working on `tui` branch (TUI enhancements with Spectre.Console).
