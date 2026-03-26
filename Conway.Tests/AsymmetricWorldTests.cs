using Conway.Domain;

namespace Conway.Tests;

public class AsymmetricWorldTests
{
    [Fact]
    public void AdvanceGeneration_WithAsymmetricWorld_ShouldNotThrow()
    {
        // Arrange
        var width = 10;
        var height = 5;
        var world = new World(width, height);
        
        // Act & Assert
        var exception = Record.Exception(() => world.AdvanceGeneration());
        Assert.Null(exception);
    }

    [Fact]
    public void GetCell_WithAsymmetricWorld_ShouldAccessCorrectCoordinates()
    {
        // Arrange
        var width = 10;
        var height = 5;
        var world = new World(width, height);
        
        // Act
        world.ToggleCellState(9, 4); // Last cell
        
        // Assert
        Assert.True(world.GetCell(9, 4).IsAlive);
    }
}
