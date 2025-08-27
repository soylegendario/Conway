using AutoFixture;
using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class CellTests
{
    private readonly Fixture _fixture;

    public CellTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Constructor_ShouldInitializeCellWithCorrectValues()
    {
        // Arrange
        var x = _fixture.Create<int>();
        var y = _fixture.Create<int>();
        var isAlive = _fixture.Create<bool>();

        // Act
        var cell = new Cell(x, y, isAlive);

        // Assert
        Assert.Equal(x, cell.X);
        Assert.Equal(y, cell.Y);
        Assert.Equal(isAlive, cell.IsAlive);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ToggleState_ShouldChangeIsAliveState(bool initialState)
    {
        // Arrange
        var cell = new Cell(0, 0, initialState);

        // Act
        cell.ToggleState();

        // Assert
        Assert.Equal(!initialState, cell.IsAlive);
    }

    [Fact]
    public void ToggleState_CalledTwice_ShouldReturnToOriginalState()
    {
        // Arrange
        var initialState = _fixture.Create<bool>();
        var cell = new Cell(0, 0, initialState);

        // Act
        cell.ToggleState();
        cell.ToggleState();

        // Assert
        Assert.Equal(initialState, cell.IsAlive);
    }

    [Fact]
    public void X_Property_ShouldBeSettable()
    {
        // Arrange
        var cell = new Cell(0, 0, false);
        var newX = _fixture.Create<int>();

        // Act
        cell.X = newX;

        // Assert
        Assert.Equal(newX, cell.X);
    }

    [Fact]
    public void Y_Property_ShouldBeSettable()
    {
        // Arrange
        var cell = new Cell(0, 0, false);
        var newY = _fixture.Create<int>();

        // Act
        cell.Y = newY;

        // Assert
        Assert.Equal(newY, cell.Y);
    }
}