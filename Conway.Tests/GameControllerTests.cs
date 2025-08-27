using AutoFixture;
using Conway.API.Controllers;
using Conway.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Conway.Tests;

public class GameControllerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IGameGrid> _mockGameGrid;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _fixture = new Fixture();
        _mockGameGrid = new Mock<IGameGrid>();
        _controller = new GameController(_mockGameGrid.Object);
    }

    [Fact]
    public async Task NewGame_ShouldReturnOkWithGameId()
    {
        // Arrange
        var width = _fixture.Create<int>();
        var height = _fixture.Create<int>();
        var expectedGameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(x => x.NewGame(width, height))
                    .ReturnsAsync(expectedGameId);

        // Act
        var result = await _controller.NewGame(width, height);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);
        
        var gameIdProperty = response.GetType().GetProperty("GameId");
        Assert.NotNull(gameIdProperty);
        Assert.Equal(expectedGameId, gameIdProperty.GetValue(response));
    }

    [Fact]
    public void GetWorld_WithValidGameId_ShouldReturnOkWithGameStatus()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        var gameStatus = new GameStatus(5, 5, new int[5, 5], 0);
        
        _mockGameGrid.Setup(x => x.GetWorld(gameId))
                    .Returns(gameStatus);

        // Act
        var result = _controller.GetWorld(gameId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void GetWorld_WithInvalidGameId_ShouldReturnNotFound()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(x => x.GetWorld(gameId))
                    .Returns((GameStatus?)null);

        // Act
        var result = _controller.GetWorld(gameId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ToggleCellState_WithValidParameters_ShouldReturnNoContent()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        var x = _fixture.Create<int>();
        var y = _fixture.Create<int>();
        
        _mockGameGrid.Setup(g => g.ToggleCellState(gameId, x, y))
                    .Returns(true);

        // Act
        var result = _controller.ToggleCellState(gameId, x, y);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void ToggleCellState_WithInvalidParameters_ShouldReturnBadRequest()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        var x = _fixture.Create<int>();
        var y = _fixture.Create<int>();
        
        _mockGameGrid.Setup(g => g.ToggleCellState(gameId, x, y))
                    .Returns(false);

        // Act
        var result = _controller.ToggleCellState(gameId, x, y);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public void AdvanceGeneration_WithValidGameId_ShouldReturnNoContent()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.AdvanceGeneration(gameId))
                    .Returns(true);

        // Act
        var result = _controller.AdvanceGeneration(gameId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void AdvanceGeneration_WithInvalidGameId_ShouldReturnNotFound()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.AdvanceGeneration(gameId))
                    .Returns(false);

        // Act
        var result = _controller.AdvanceGeneration(gameId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public void Shuffle_WithValidGameId_ShouldReturnNoContent()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.Shuffle(gameId))
                    .Returns(true);

        // Act
        var result = _controller.Shuffle(gameId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Shuffle_WithInvalidGameId_ShouldReturnNotFound()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.Shuffle(gameId))
                    .Returns(false);

        // Act
        var result = _controller.Shuffle(gameId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public void UndoGeneration_WithValidGameIdAndHistory_ShouldReturnNoContent()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.UndoGeneration(gameId))
                    .Returns(true);

        // Act
        var result = _controller.UndoGeneration(gameId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void UndoGeneration_WithInvalidGameIdOrNoHistory_ShouldReturnBadRequest()
    {
        // Arrange
        var gameId = _fixture.Create<string>();
        
        _mockGameGrid.Setup(g => g.UndoGeneration(gameId))
                    .Returns(false);

        // Act
        var result = _controller.UndoGeneration(gameId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequestResult.Value);
    }
}