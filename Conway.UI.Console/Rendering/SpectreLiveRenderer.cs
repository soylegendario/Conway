using Conway.Domain;
using Spectre.Console;

namespace Conway.UI.Console.Rendering;

public class SpectreLiveRenderer : IGameRenderer
{
    public async Task StartRenderLoopAsync(IGameGrid grid, string gameId, int tickDelayMs)
    {
        await AnsiConsole.Live(new Text("Cargando..."))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    var status = grid.GetWorld(gameId);
                    if (status == null) break;

                    var canvas = new Canvas(status.Width, status.Height);
                    for (int y = 0; y < status.Height; y++)
                    {
                        for (int x = 0; x < status.Width; x++)
                        {
                            if (status.Cells[y, x] == 1)
                            {
                                canvas.SetPixel(x, y, Color.Green);
                            }
                            else
                            {
                                canvas.SetPixel(x, y, Color.Black);
                            }
                        }
                    }

                    var table = new Table().Centered();
                    table.AddColumn(new TableColumn("[yellow]Conway's Game of Life[/]").Centered());
                    
                    var stats = $"[blue]Game ID:[/] {gameId} | [blue]Generación:[/] {status.Ticks} | [blue]Delay:[/] {tickDelayMs}ms";
                    table.AddRow(new Panel(stats).BorderColor(Color.Blue).Header("Estadísticas"));
                    table.AddRow(canvas);

                    ctx.UpdateTarget(table);
                    ctx.Refresh();

                    grid.AdvanceGeneration(gameId);
                    await Task.Delay(tickDelayMs);
                }
            });
    }
}
