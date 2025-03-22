using Conway.Api.Dto;
using Conway.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Conway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(GameGrid gameGrid) : ControllerBase
{
    [HttpPost("new")]
    public async Task<IActionResult> NewGame([FromQuery] int width, [FromQuery] int height)
    {
        var gameId = await gameGrid.NewGame(width, height);
        return Ok(new { GameId = gameId });
    }

    [HttpGet("{gameId}")]
    public IActionResult GetWorld([FromRoute] string gameId)
    {
        var gameStatus = gameGrid.GetWorld(gameId);
        if (gameStatus == null)
        {
            return NotFound();
        }
        var dto = new GameStatusDto(gameStatus.Height, gameStatus.Width, gameStatus.Cells, gameStatus.Ticks);
        
        return Ok(dto);
    }

    [HttpPost("{gameId}/toggle")]
    public IActionResult ToggleCellState([FromRoute] string gameId, [FromQuery] int x, [FromQuery] int y)
    {
        gameGrid.ToggleCellState(gameId, x, y);
        return NoContent();
    }

    [HttpPost("{gameId}/advance")]
    public IActionResult AdvanceGeneration([FromRoute] string gameId)
    {
        gameGrid.AdvanceGeneration(gameId);
        return NoContent();
    }

    [HttpPost("{gameId}/shuffle")]
    public IActionResult Shuffle(string gameId)
    {
        gameGrid.Shuffle(gameId);
        return NoContent();
    }
}
