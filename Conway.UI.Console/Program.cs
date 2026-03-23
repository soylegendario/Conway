using Conway.Domain;
using Conway.UI.Console.Rendering;
using Spectre.Console;

var continueRunning = true;
while (continueRunning)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(
        new FigletText("Conway's Game of Life")
            .Centered()
            .Color(Color.Green));

    var width = AnsiConsole.Prompt(
        new TextPrompt<int>("[white]Introduce el [green]ancho[/] del mundo (10-100):[/]")
            .ValidationErrorMessage("[red]Por favor, introduce un número válido entre 10 y 100[/]")
            .Validate(n => n switch
            {
                < 10 => ValidationResult.Error("[red]El ancho debe ser al menos 10[/]"),
                > 100 => ValidationResult.Error("[red]El ancho no puede ser mayor a 100[/]"),
                _ => ValidationResult.Success(),
            }));

    var height = AnsiConsole.Prompt(
        new TextPrompt<int>("[white]Introduce el [green]alto[/] del mundo (10-100):[/]")
            .ValidationErrorMessage("[red]Por favor, introduce un número válido entre 10 y 100[/]")
            .Validate(n => n switch
            {
                < 10 => ValidationResult.Error("[red]El alto debe ser al menos 10[/]"),
                > 100 => ValidationResult.Error("[red]El alto no puede ser mayor a 100[/]"),
                _ => ValidationResult.Success(),
            }));

    var grid = new GameGrid();
    var gameId = await grid.NewGame(width, height);
    grid.Shuffle(gameId);

    var renderer = new SpectreLiveRenderer();
    continueRunning = await renderer.StartRenderLoopAsync(grid, gameId, 100);
}
