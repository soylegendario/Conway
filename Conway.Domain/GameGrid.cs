using System.Collections.Concurrent;

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
        if (_worlds.TryGetValue(gameId, out var world))
        {
            var cells = new int[world.Cells.GetLength(0), world.Cells.GetLength(1)];
            for (int i = 0; i < world.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < world.Cells.GetLength(1); j++)
                {
                    cells[i, j] = world.Cells[i, j].IsAlive ? 1 : 0;
                }
            }
            return new GameStatus(world.Height, world.Width, cells, world.GenerationHistory.Count);
        }

        return null;
    }

    public bool ToggleCellState(string gameId, int x, int y)
    {
        if (_worlds.TryGetValue(gameId, out var world))
        {
            return world.ToggleCellState(x, y);
        }
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
        if (_worlds.TryGetValue(gameId, out var world))
        {
            world.Initialize();
            var random = new Random();
            for (int i = 0; i < world.Cells.GetLength(0); i++)
            {
                for (int j = 0; j < world.Cells.GetLength(1); j++)
                {
                    if (random.Next(2) == 1)
                    {
                        ToggleCellState(gameId, i, j);
                    }
                }
            }
            return true;
        }
        return false;
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
        var hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(guid));
        return Convert.ToBase64String(hash).Replace("/", "_").Replace("+", "-");
    }
}
