# Conway's Game of Life Development Guidelines

This document provides essential information for advanced developers working on the Conway project.

## Build and Configuration

The project is built using **.NET 8.0**. It is organized as a multi-project solution:

- **Conway.Domain**: Core logic and domain models (`World`, `Cell`).
- **Conway.Tests**: Unit and integration tests using xUnit.
- **Conway.UI.Api**: ASP.NET Core Web API interface.
- **Conway.UI.Console**: Console-based UI using Spectre.Console.

### Build Instructions
To build the entire solution from the root directory:
```bash
dotnet build
```

To run a specific project (e.g., the API):
```bash
dotnet run --project Conway.UI.Api
```

## Testing Information

Tests are implemented using **xUnit** and **AutoFixture** for data generation.

### Running Tests
To run all tests in the solution:
```bash
dotnet test
```

To run tests in a specific file or filter by name:
```bash
dotnet test --filter "FullyQualifiedName~Conway.Tests.WorldTests"
```

### Adding New Tests
When adding new tests, follow the existing pattern in `Conway.Tests`. Use `AutoFixture` when you need to generate complex test data.

**Example of a simple test:**
```csharp
using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class SampleTest
{
    [Fact]
    public void World_ShouldToggleStateCorrectly()
    {
        // Arrange
        var world = new World(5, 5);
        
        // Act
        world.ToggleCellState(2, 2);
        
        // Assert
        Assert.True(world.Cells[2, 2].IsAlive);
    }
}
```

## Additional Development Information

### Code Style
- Follow standard C# coding conventions and .NET 8 features (e.g., file-scoped namespaces, primary constructors where appropriate).
- The domain models (`World`, `Cell`) are designed to be self-contained and encapsulate their state transitions.
- Use `IGameGrid` abstraction when interacting with the simulation state from UI layers.

### UI Layers
- **Web API**: Uses standard ASP.NET Core controllers and is configured with Swagger for easy testing during development.
- **Console UI**: Leverages `Spectre.Console` for rich terminal rendering.

### Evolution
The project is designed to be extensible. To add a new UI, implement an interface that interacts with the `Conway.Domain` or `IGameGrid` abstraction.
