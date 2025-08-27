using Conway.Api.Dto;
using Conway.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Conway.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController(IGameGrid gameGrid) : ControllerBase
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
        var success = gameGrid.ToggleCellState(gameId, x, y);
        if (!success)
        {
            return BadRequest(new { message = "Coordenadas inválidas o el juego no existe." });
        }
        return NoContent();
    }

    [HttpPost("{gameId}/advance")]
    public IActionResult AdvanceGeneration([FromRoute] string gameId)
    {
        var success = gameGrid.AdvanceGeneration(gameId);
        if (!success)
        {
            return NotFound(new { message = "El juego no existe." });
        }
        return NoContent();
    }

    [HttpPost("{gameId}/shuffle")]
    public IActionResult Shuffle(string gameId)
    {
        var success = gameGrid.Shuffle(gameId);
        if (!success)
        {
            return NotFound(new { message = "El juego no existe." });
        }
        return NoContent();
    }

    [HttpPost("{gameId}/undo")]
    public IActionResult UndoGeneration([FromRoute] string gameId)
    {
        var success = gameGrid.UndoGeneration(gameId);
        if (!success)
        {
            return BadRequest(new { message = "No hay generaciones anteriores para deshacer o el juego no existe." });
        }
        return NoContent();
    }
}
