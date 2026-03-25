using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class WorldTests
{
    [Fact]
    public void Constructor_ShouldInitializeWorldWithCorrectDimensions()
    {
        var width = 10;
        var height = 15;

        var world = new World(width, height);

        Assert.Equal(width, world.Width);
        Assert.Equal(height, world.Height);
    }

    [Fact]
    public void Initialize_ShouldCreateAllDeadCells()
    {
        var world = new World(5, 5);

        world.Initialize();

        for (var x = 0; x < world.Width; x++)
        {
            for (var y = 0; y < world.Height; y++)
            {
                Assert.False(world.GetCell(x, y).IsAlive);
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
        var world = new World(5, 5);
        var initialState = world.GetCell(x, y).IsAlive;

        var result = world.ToggleCellState(x, y);

        Assert.True(result);
        Assert.NotEqual(initialState, world.GetCell(x, y).IsAlive);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    [InlineData(10, 10)]
    public void ToggleCellState_WithInvalidCoordinates_ShouldReturnFalse(int x, int y)
    {
        var world = new World(5, 5);

        var result = world.ToggleCellState(x, y);

        Assert.False(result);
    }

    [Fact]
    public void AdvanceGeneration_ShouldApplyConwayRules()
    {
        var world = new World(3, 3);

        // Blinker pattern (vertical)
        world.ToggleCellState(1, 0);
        world.ToggleCellState(1, 1);
        world.ToggleCellState(1, 2);

        world.AdvanceGeneration();

        // After one generation blinker rotates 90 degrees (horizontal)
        Assert.False(world.GetCell(1, 0).IsAlive);
        Assert.True(world.GetCell(0, 1).IsAlive);
        Assert.True(world.GetCell(1, 1).IsAlive);
        Assert.True(world.GetCell(2, 1).IsAlive);
        Assert.False(world.GetCell(1, 2).IsAlive);
        Assert.Single(world.GenerationHistory);
    }

    [Fact]
    public void UndoGeneration_WithHistory_ShouldRestorePreviousState()
    {
        var world = new World(3, 3);
        world.ToggleCellState(1, 1);
        var originalState = world.GetCell(1, 1).IsAlive;

        world.AdvanceGeneration();
        Assert.False(world.GetCell(1, 1).IsAlive);
        Assert.Single(world.GenerationHistory);

        world.UndoGeneration();

        Assert.Equal(originalState, world.GetCell(1, 1).IsAlive);
        Assert.Empty(world.GenerationHistory);
    }

    [Fact]
    public void UndoGeneration_WithoutHistory_ShouldNotChangeState()
    {
        var world = new World(3, 3);
        world.ToggleCellState(1, 1);
        var originalState = world.GetCell(1, 1).IsAlive;

        world.UndoGeneration();

        Assert.Equal(originalState, world.GetCell(1, 1).IsAlive);
        Assert.Empty(world.GenerationHistory);
    }

    [Fact]
    public void GetGenerationCount_ShouldReturnCorrectCount()
    {
        var world = new World(3, 3);
        Assert.Equal(0, world.GetGenerationCount());

        world.AdvanceGeneration();
        Assert.Equal(1, world.GetGenerationCount());

        world.AdvanceGeneration();
        Assert.Equal(2, world.GetGenerationCount());

        world.UndoGeneration();
        Assert.Equal(1, world.GetGenerationCount());
    }
}
