using Conway.Domain;

namespace Conway.Tests;

public class GameGridAsymmetricTests
{
    [Theory]
    [InlineData(20, 10)]
    [InlineData(10, 20)]
    [InlineData(30, 10)]
    [InlineData(10, 30)]
    public async Task GetWorld_WithAsymmetricDimensions_CellsAreIndexedRowColumn(int width, int height)
    {
        // Arrange
        var grid = new GameGrid();
        var gameId = await grid.NewGame(width, height);

        // Act
        var status = grid.GetWorld(gameId)!;

        // Assert: dimensiones correctas
        Assert.Equal(width, status.Width);
        Assert.Equal(height, status.Height);

        // Assert: acceso [y, x] no lanza excepción (Cells es [Height, Width])
        var exception = Record.Exception(() =>
        {
            for (var y = 0; y < status.Height; y++)
            for (var x = 0; x < status.Width; x++)
                _ = status.Cells[y, x];
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task GetWorld_Width10_Height20_CellsDimensionsAreHeightByWidth()
    {
        var grid = new GameGrid();
        var gameId = await grid.NewGame(10, 20);
        var status = grid.GetWorld(gameId)!;

        // Cells debe ser [20, 10] — primera dimensión es Height
        Assert.Equal(20, status.Cells.GetLength(0));
        Assert.Equal(10, status.Cells.GetLength(1));
    }

    [Fact]
    public async Task GetWorld_ToggleCell_ReflectsCorrectlyInRowColumnMatrix()
    {
        var grid = new GameGrid();
        var gameId = await grid.NewGame(10, 20);
        grid.ToggleCellState(gameId, 5, 15); // x=5, y=15

        var status = grid.GetWorld(gameId)!;

        Assert.Equal(1, status.Cells[15, 5]); // Cells[y, x]
    }
}
