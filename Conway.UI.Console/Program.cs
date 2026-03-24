using Conway.Domain;
using Conway.UI.Console.Rendering;
using Spectre.Console;

const string OPTION_NEW_GAME = "[green]1. Nuevo juego[/]";
const string OPTION_LOAD_GAME = "[yellow]2. Cargar juego[/]";
const string OPTION_EXIT = "[red]0. Salir[/]";

const string SETUP_OPTION_SHUFFLE = "1. Colocar células automáticamente";
const string SETUP_OPTION_START   = "2. Iniciar";
const string SETUP_OPTION_BACK    = "0. Volver al menú principal";

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

    var setupStatus = grid.GetWorld(gameId)!;
    RenderSetupBoard(setupStatus, gameId);

    var inSetup = true;
    while (inSetup)
    {
        var setupOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Configuración del juego[/]")
                .PageSize(5)
                .AddChoices(SETUP_OPTION_SHUFFLE, SETUP_OPTION_START, SETUP_OPTION_BACK));

        switch (setupOption)
        {
            case SETUP_OPTION_SHUFFLE:
                grid.Shuffle(gameId);
                setupStatus = grid.GetWorld(gameId)!;
                RenderSetupBoard(setupStatus, gameId);
                break;

            case SETUP_OPTION_START:
                inSetup = false;
                var renderer = new SpectreLiveRenderer();
                await renderer.StartRenderLoopAsync(grid, gameId, 100);
                break;

            case SETUP_OPTION_BACK:
                inSetup = false;
                break;
        }
    }
}

void RenderSetupBoard(GameStatus status, string gameId)
{
    AnsiConsole.Clear();

    var canvas = new Canvas(status.Width, status.Height);
    for (var y = 0; y < status.Height; y++)
    {
        for (var x = 0; x < status.Width; x++)
        {
            canvas.SetPixel(x, y, status.Cells[y, x] == 1 ? Color.Green : Color.Black);
        }
    }

    var table = new Table().Centered();
    table.AddColumn(new TableColumn("[yellow]Conway's Game of Life — Configuración[/]").Centered());

    var info = $"[blue]Game ID:[/] {gameId} | [grey]Elige una opción para continuar[/]";
    table.AddRow(new Panel(info).BorderColor(Color.Blue).Header("Configuración"));
    table.AddRow(canvas);

    AnsiConsole.Write(table);
}
