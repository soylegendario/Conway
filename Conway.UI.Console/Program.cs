using Conway.Domain;

namespace Conway.UI.Console;

public class Program
{
    static void Main()
    {
        System.Console.WriteLine("Welcome to Conway's Game of Life");
        System.Console.Write("Enter the width of the world: ");
        var width = int.Parse(System.Console.ReadLine() ?? "5");
        System.Console.Write("Enter the height of the world: ");
        var height = int.Parse(System.Console.ReadLine() ?? "5");

        var gameId = GameGrid.NewGame(width, height);

        while (true)
        {
            System.Console.WriteLine("\nOptions:");
            System.Console.WriteLine("1. Display the world");
            System.Console.WriteLine("2. Advance one generation");
            System.Console.WriteLine("3. Toggle cell state");
            System.Console.WriteLine("4. Shuffle");
            System.Console.WriteLine("5. Auto play");
            System.Console.WriteLine("0. Exit");
            System.Console.Write("Select an option: ");
            var option = System.Console.ReadLine();

            switch (option)
            {
                case "1":
                    PrintWorld(gameId);
                    break;
                case "2":
                    GameGrid.AdvanceGeneration(gameId);
                    PrintWorld(gameId);
                    break;
                case "3":
                    System.Console.Write("Enter the X coordinate of the cell: ");
                    var x = int.Parse(System.Console.ReadLine() ?? "0");
                    System.Console.Write("Enter the Y coordinate of the cell: ");
                    var y = int.Parse(System.Console.ReadLine() ?? "0");
                    GameGrid.ToggleCellState(gameId, x, y);
                    System.Console.WriteLine("Cell state toggled.");
                    break;
                case "4":
                    GameGrid.Shuffle(gameId);
                    PrintWorld(gameId);
                    break;
                case "5":
                    System.Console.WriteLine("Press ESC to stop auto play...");
                    while (true)
                    {
                        if (System.Console.KeyAvailable && System.Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        PrintWorld(gameId);
                        GameGrid.AdvanceGeneration(gameId);
                        System.Threading.Thread.Sleep(500);
                    }
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void PrintWorld(string gameId)
    {
        System.Console.WriteLine($"Game ID: {gameId}");
        var gameStatus = GameGrid.GetWorld(gameId);
        if (gameStatus is null)
        {
            System.Console.WriteLine("Invalid game ID");
            return;
        }
        var cells = gameStatus.Cells;
        for (var y = 0; y < gameStatus.Height; y++)
        {
            for (var x = 0; x < gameStatus.Width; x++)
            {
                System.Console.Write(cells[x, y] is 0 ? "-" : "*");
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine($"{gameStatus.Ticks} ticks");
    }
}