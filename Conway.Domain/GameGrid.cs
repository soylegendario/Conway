using System.Collections.Concurrent;

namespace Conway.Domain;

public class GameGrid : IGameGrid
{
    private readonly ConcurrentDictionary<string, World> _worlds = new();

    public Task<string> NewGame(int width, int height)
    {
        var world = new World(width, height);
        var id = GenerateUniqueGameId();
        _worlds.TryAdd(id, world);
        return Task.FromResult(id);
    }

    public GameStatus? GetWorld(string gameId)
    {
        if (!_worlds.TryGetValue(gameId, out var world)) return null;

        var cells = new int[world.Height, world.Width];
        for (var y = 0; y < world.Height; y++)
        {
            for (var x = 0; x < world.Width; x++)
            {
                cells[y, x] = world.GetCell(x, y).IsAlive ? 1 : 0;
            }
        }
        return new GameStatus(world.Height, world.Width, cells, world.GenerationHistory.Count);
    }

    public bool ToggleCellState(string gameId, int x, int y)
    {
        if (_worlds.TryGetValue(gameId, out var world))
            return world.ToggleCellState(x, y);
        return false;
    }

    public bool AdvanceGeneration(string gameId)
    {
        if (_worlds.TryGetValue(gameId, out var world))
        {
            world.AdvanceGeneration();
            return true;
        }
        return false;
    }

    public bool Shuffle(string gameId)
    {
        if (!_worlds.TryGetValue(gameId, out var world)) return false;

        world.Initialize();
        var random = new Random();
        for (var x = 0; x < world.Width; x++)
        {
            for (var y = 0; y < world.Height; y++)
            {
                if (random.Next(2) == 1)
                    world.ToggleCellState(x, y);
            }
        }
        return true;
    }

    public bool UndoGeneration(string gameId)
    {
        if (_worlds.TryGetValue(gameId, out var world))
        {
            var generationCount = world.GetGenerationCount();
            world.UndoGeneration();
            return world.GetGenerationCount() < generationCount;
        }
        return false;
    }

    private string GenerateUniqueGameId()
    {
        string id;
        do
        {
            id = GenerateUniqueId();
        } while (_worlds.ContainsKey(id));
        return id;
    }

    private string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString("N");
    }
}
