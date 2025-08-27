using AutoFixture;
using Conway.Domain;
using Xunit;

namespace Conway.Tests;

public class GameGridTests
{
    private readonly Fixture _fixture;
    private readonly GameGrid _gameGrid;

    public GameGridTests()
    {
        _fixture = new Fixture();
        _gameGrid = new GameGrid();
    }

    [Fact]
    public async Task NewGame_ShouldCreateGameWithUniqueId()
    {
        // Arrange
        var width = _fixture.Create<int>() % 50 + 1; // Ensure positive
        var height = _fixture.Create<int>() % 50 + 1;

        // Act
        var gameId = await _gameGrid.NewGame(width, height);

        // Assert
        Assert.NotNull(gameId);
        Assert.NotEmpty(gameId);
        
        var gameStatus = _gameGrid.GetWorld(gameId);
        Assert.NotNull(gameStatus);
        Assert.Equal(width, gameStatus.Width);
        Assert.Equal(height, gameStatus.Height);
    }

    [Fact]
    public async Task NewGame_MultipleCalls_ShouldCreateUniqueIds()
    {
        // Arrange & Act
        var gameId1 = await _gameGrid.NewGame(5, 5);
        var gameId2 = await _gameGrid.NewGame(5, 5);

        // Assert
        Assert.NotEqual(gameId1, gameId2);
    }

    [Fact]
    public async Task GetWorld_WithValidGameId_ShouldReturnGameStatus()
    {
        // Arrange
        var width = 3;
        var height = 4;
        var gameId = await _gameGrid.NewGame(width, height);

        // Act
        var result = _gameGrid.GetWorld(gameId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(width, result.Width);
        Assert.Equal(height, result.Height);
        Assert.Equal(0, result.Ticks);
        Assert.Equal(width, result.Cells.GetLength(0));
        Assert.Equal(height, result.Cells.GetLength(1));
    }

    [Fact]
    public void GetWorld_WithInvalidGameId_ShouldReturnNull()
    {
        // Arrange
        var invalidGameId = _fixture.Create<string>();

        // Act
        var result = _gameGrid.GetWorld(invalidGameId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ToggleCellState_WithValidGameIdAndCoordinates_ShouldReturnTrue()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);
        var x = 2;
        var y = 3;

        // Act
        var result = _gameGrid.ToggleCellState(gameId, x, y);

        // Assert
        Assert.True(result);
        
        var gameStatus = _gameGrid.GetWorld(gameId);
        Assert.Equal(1, gameStatus!.Cells[x, y]); // Cell should be alive
    }

    [Fact]
    public void ToggleCellState_WithInvalidGameId_ShouldReturnFalse()
    {
        // Arrange
        var invalidGameId = _fixture.Create<string>();

        // Act
        var result = _gameGrid.ToggleCellState(invalidGameId, 0, 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ToggleCellState_WithInvalidCoordinates_ShouldReturnFalse()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);

        // Act
        var result = _gameGrid.ToggleCellState(gameId, -1, 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AdvanceGeneration_WithValidGameId_ShouldReturnTrue()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);
        var initialTicks = _gameGrid.GetWorld(gameId)!.Ticks;

        // Act
        var result = _gameGrid.AdvanceGeneration(gameId);

        // Assert
        Assert.True(result);
        
        var gameStatus = _gameGrid.GetWorld(gameId);
        Assert.Equal(initialTicks + 1, gameStatus!.Ticks);
    }

    [Fact]
    public void AdvanceGeneration_WithInvalidGameId_ShouldReturnFalse()
    {
        // Arrange
        var invalidGameId = _fixture.Create<string>();

        // Act
        var result = _gameGrid.AdvanceGeneration(invalidGameId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Shuffle_WithValidGameId_ShouldReturnTrue()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);

        // Act
        var result = _gameGrid.Shuffle(gameId);

        // Assert
        Assert.True(result);
        
        // Verify that some cells might be alive after shuffle
        var gameStatus = _gameGrid.GetWorld(gameId);
        Assert.NotNull(gameStatus);
    }

    [Fact]
    public void Shuffle_WithInvalidGameId_ShouldReturnFalse()
    {
        // Arrange
        var invalidGameId = _fixture.Create<string>();

        // Act
        var result = _gameGrid.Shuffle(invalidGameId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UndoGeneration_WithValidGameIdAndHistory_ShouldReturnTrue()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);
        _gameGrid.ToggleCellState(gameId, 2, 2); // Create some initial state
        _gameGrid.AdvanceGeneration(gameId); // Create history
        
        var ticksBeforeUndo = _gameGrid.GetWorld(gameId)!.Ticks;

        // Act
        var result = _gameGrid.UndoGeneration(gameId);

        // Assert
        Assert.True(result);
        
        var gameStatus = _gameGrid.GetWorld(gameId);
        Assert.True(gameStatus!.Ticks < ticksBeforeUndo);
    }

    [Fact]
    public async Task UndoGeneration_WithValidGameIdButNoHistory_ShouldReturnFalse()
    {
        // Arrange
        var gameId = await _gameGrid.NewGame(5, 5);

        // Act
        var result = _gameGrid.UndoGeneration(gameId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void UndoGeneration_WithInvalidGameId_ShouldReturnFalse()
    {
        // Arrange
        var invalidGameId = _fixture.Create<string>();

        // Act
        var result = _gameGrid.UndoGeneration(invalidGameId);

        // Assert
        Assert.False(result);
    }
}