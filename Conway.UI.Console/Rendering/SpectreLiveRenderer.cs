using Conway.Domain;
using Spectre.Console;

namespace Conway.UI.Console.Rendering;

public class SpectreLiveRenderer : IGameRenderer
{
    public async Task<bool> StartRenderLoopAsync(IGameGrid grid, string gameId, int tickDelayMs)
    {
        var shouldRestart = false;
        var shouldExit = false;

        await AnsiConsole.Live(new Text("Cargando..."))
            .StartAsync(async ctx =>
            {
                while (!shouldExit)
                {
                    if (System.Console.KeyAvailable)
                    {
                        var key = System.Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Q)
                        {
                            shouldExit = true;
                            break;
                        }
                    }

                    var status = grid.GetWorld(gameId);
                    if (status == null)
                    {
                        shouldExit = true;
                        break;
                    }

                    var canvas = new Canvas(status.Width, status.Height);
                    for (var y = 0; y < status.Height; y++)
                    {
                        for (var x = 0; x < status.Width; x++)
                        {
                            canvas.SetPixel(x, y, status.Cells[y, x] == 1 ? Color.Green : Color.Black);
                        }
                    }

                    var table = new Table().Centered();
                    table.AddColumn(new TableColumn("[yellow]Conway's Game of Life[/]").Centered());
                    
                    var stats = $"[blue]Game ID:[/] {gameId} | [blue]Generación:[/] {status.Ticks} | [blue]Delay:[/] {tickDelayMs}ms\n[grey]Pulsa 'Q' para volver al inicio[/]";
                    table.AddRow(new Panel(stats).BorderColor(Color.Blue).Header("Estadísticas"));
                    table.AddRow(canvas);

                    ctx.UpdateTarget(table);
                    ctx.Refresh();

                    grid.AdvanceGeneration(gameId);
                    await Task.Delay(tickDelayMs);
                }
            });

        if (!shouldExit) // Si salimos por otra razón que no sea el break interno
        {
             // En este caso, el loop terminó normalmente o el mundo desapareció.
        }
        
        // Mostrar el prompt fuera del contexto de LiveDisplay para evitar el error de concurrencia
        if (AnsiConsole.Confirm("¿Quieres volver al inicio e iniciar un nuevo juego?", true))
        {
            shouldRestart = true;
        }

        return shouldRestart;
    }
}
