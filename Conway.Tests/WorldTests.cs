using AutoFixture;
using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class WorldTests
{
    private readonly Fixture _fixture;

    public WorldTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Constructor_ShouldInitializeWorldWithCorrectDimensions()
    {
        // Arrange
        var width = 10;
        var height = 15;

        // Act
        var world = new World(width, height);

        // Assert
        Assert.Equal(width, world.Width);
        Assert.Equal(height, world.Height);
        Assert.Equal(width, world.Cells.GetLength(0));
        Assert.Equal(height, world.Cells.GetLength(1));
    }

    [Fact]
    public void Initialize_ShouldCreateAllDeadCells()
    {
        // Arrange
        var world = new World(5, 5);

        // Act
        world.Initialize();

        // Assert
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Assert.False(world.Cells[x, y].IsAlive);
            }
        }
        Assert.Empty(world.GenerationHistory);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 3)]
    [InlineData(4, 4)]
    public void ToggleCellState_WithValidCoordinates_ShouldToggleCell(int x, int y)
    {
        // Arrange
        var world = new World(5, 5);
        var initialState = world.Cells[x, y].IsAlive;

        // Act
        var result = world.ToggleCellState(x, y);

        // Assert
        Assert.True(result);
        Assert.NotEqual(initialState, world.Cells[x, y].IsAlive);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    [InlineData(10, 10)]
    public void ToggleCellState_WithInvalidCoordinates_ShouldReturnFalse(int x, int y)
    {
        // Arrange
        var world = new World(5, 5);

        // Act
        var result = world.ToggleCellState(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AdvanceGeneration_ShouldApplyConwayRules()
    {
        // Arrange
        var world = new World(3, 3);
        
        // Create a blinker pattern (oscillator)
        world.ToggleCellState(1, 0); // Top center
        world.ToggleCellState(1, 1); // Center
        world.ToggleCellState(1, 2); // Bottom center

        // Act
        world.AdvanceGeneration();

        // Assert
        // After one generation, blinker should rotate 90 degrees
        Assert.False(world.Cells[1, 0].IsAlive); // Top center should be dead
        Assert.True(world.Cells[0, 1].IsAlive);  // Left center should be alive
        Assert.True(world.Cells[1, 1].IsAlive);  // Center should remain alive
        Assert.True(world.Cells[2, 1].IsAlive);  // Right center should be alive
        Assert.False(world.Cells[1, 2].IsAlive); // Bottom center should be dead
        
        // Generation history should contain previous state
        Assert.Single(world.GenerationHistory);
    }

    [Fact]
    public void UndoGeneration_WithHistory_ShouldRestorePreviousState()
    {
        // Arrange
        var world = new World(3, 3);
        world.ToggleCellState(1, 1); // Set center cell alive
        var originalState = world.Cells[1, 1].IsAlive;
        
        world.AdvanceGeneration(); // This will kill the isolated cell
        Assert.False(world.Cells[1, 1].IsAlive); // Verify cell died
        Assert.Single(world.GenerationHistory);

        // Act
        world.UndoGeneration();

        // Assert
        Assert.Equal(originalState, world.Cells[1, 1].IsAlive);
        Assert.Empty(world.GenerationHistory);
    }

    [Fact]
    public void UndoGeneration_WithoutHistory_ShouldNotChangeState()
    {
        // Arrange
        var world = new World(3, 3);
        world.ToggleCellState(1, 1);
        var originalState = world.Cells[1, 1].IsAlive;

        // Act
        world.UndoGeneration();

        // Assert
        Assert.Equal(originalState, world.Cells[1, 1].IsAlive);
        Assert.Empty(world.GenerationHistory);
    }

    [Fact]
    public void GetGenerationCount_ShouldReturnCorrectCount()
    {
        // Arrange
        var world = new World(3, 3);
        Assert.Equal(0, world.GetGenerationCount());

        // Act & Assert
        world.AdvanceGeneration();
        Assert.Equal(1, world.GetGenerationCount());

        world.AdvanceGeneration();
        Assert.Equal(2, world.GetGenerationCount());

        world.UndoGeneration();
        Assert.Equal(1, world.GetGenerationCount());
    }
}