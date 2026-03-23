using Conway.Domain;
using Conway.UI.Console.Rendering;
using Spectre.Console;

const string OPTION_NEW_GAME = "[green]1. Nuevo juego[/]";
const string OPTION_LOAD_GAME = "[yellow]2. Cargar juego[/]";
const string OPTION_EXIT = "[red]0. Salir[/]";

var continueRunning = true;

while (continueRunning)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(
        new FigletText("Conway's Game of Life")
            .Centered()
            .Color(Color.Green));

    var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[yellow]Menú Principal[/]")
            .PageSize(10)
            .AddChoices(OPTION_NEW_GAME, OPTION_LOAD_GAME, OPTION_EXIT));

    switch (option)
    {
        case OPTION_NEW_GAME:
            await RunNewGame();
            break;
        case OPTION_LOAD_GAME:
            AnsiConsole.MarkupLine("[yellow]Funcionalidad 'Cargar juego' no implementada aún.[/]");
            AnsiConsole.MarkupLine("[grey]Presione cualquier tecla para volver al menú...[/]");
            Console.ReadKey(true);
            break;
        case OPTION_EXIT:
            continueRunning = false;
            break;
    }
}

async Task RunNewGame()
{
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
    await renderer.StartRenderLoopAsync(grid, gameId, 100);
}
