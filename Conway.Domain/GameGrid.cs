using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Conway.Domain;

public class GameGrid : IGameGrid
{
    private readonly ConcurrentDictionary<string, World> _worlds = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<string> NewGame(int width, int height)
    {
        await _semaphore.WaitAsync();
        try
        {
            var world = new World(width, height);
            var id = GenerateUniqueGameId();
            _worlds.TryAdd(id, world);
            return id;
        }
        finally
        {
            _semaphore.Release();
        }
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
        var guid = Guid.NewGuid().ToString();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(guid));
        return Convert.ToBase64String(hash).Replace("/", "_").Replace("+", "-");
    }
}
