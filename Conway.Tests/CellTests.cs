using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class CellTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_ShouldInitializeCellWithCorrectIsAlive(bool isAlive)
    {
        var cell = new Cell(isAlive);

        Assert.Equal(isAlive, cell.IsAlive);
    }

    [Fact]
    public void TwoCells_WithSameIsAlive_ShouldBeEqual()
    {
        var a = new Cell(true);
        var b = new Cell(true);

        Assert.Equal(a, b);
    }

    [Fact]
    public void TwoCells_WithDifferentIsAlive_ShouldNotBeEqual()
    {
        var a = new Cell(true);
        var b = new Cell(false);

        Assert.NotEqual(a, b);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Cell_WithToggledIsAlive_ShouldHaveOppositeState(bool initialState)
    {
        var cell = new Cell(initialState);
        var toggled = new Cell(!cell.IsAlive);

        Assert.Equal(!initialState, toggled.IsAlive);
    }

    [Fact]
    public void Cell_ToggledTwice_ShouldReturnToOriginalState()
    {
        var cell = new Cell(true);
        var toggled = new Cell(!new Cell(!cell.IsAlive).IsAlive);

        Assert.Equal(cell.IsAlive, toggled.IsAlive);
    }
}
