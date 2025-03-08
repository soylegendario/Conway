using Conway.Domain;

namespace Conway.Application.Console;

public class Program
{
    static void Main()
    {
        System.Console.WriteLine("Welcome to Conway's Game of Life");
        System.Console.Write("Enter the width of the world: ");
        var width = int.Parse(System.Console.ReadLine() ?? "5");
        System.Console.Write("Enter the height of the world: ");
        var height = int.Parse(System.Console.ReadLine() ?? "5");

        var world = new World(width, height);

        while (true)
        {
            System.Console.WriteLine("\nOptions:");
            System.Console.WriteLine("1. Display the world");
            System.Console.WriteLine("2. Advance one generation");
            System.Console.WriteLine("3. Toggle cell state");
            System.Console.WriteLine("4. Exit");
            System.Console.Write("Select an option: ");
            var option = System.Console.ReadLine();

            switch (option)
            {
                case "1":
                    PrintWorld(world);
                    break;
                case "2":
                    world.AdvanceGeneration();
                    System.Console.WriteLine($"Generation {world.GetGenerationCount()}:");
                    PrintWorld(world);
                    break;
                case "3":
                    System.Console.Write("Enter the X coordinate of the cell: ");
                    var x = int.Parse(System.Console.ReadLine() ?? "0");
                    System.Console.Write("Enter the Y coordinate of the cell: ");
                    var y = int.Parse(System.Console.ReadLine() ?? "0");
                    world.ToggleCellState(x, y);
                    System.Console.WriteLine("Cell state toggled.");
                    break;
                case "4":
                    return;
                default:
                    System.Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void PrintWorld(World world)
    {
        for (var y = 0; y < world.Height; y++)
        {
            for (var x = 0; x < world.Width; x++)
            {
                System.Console.Write(world.Cells[x, y].IsAlive ? "O" : ".");
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine($"{world.GetGenerationCount()} ticks");
    }
}