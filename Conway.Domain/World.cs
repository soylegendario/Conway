namespace Conway.Domain;

public sealed class World
{
    public int Width { get; }
    public int Height { get; }
    public Cell[,] Cells { get; private set; } = new Cell[0,0];
    public List<Cell[,]> GenerationHistory { get; private set; } = [];

    public World(int width, int height)
    {
        Width = width;
        Height = height;
        Initialize();
    }

    public bool ToggleCellState(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            Cells[x, y].ToggleState();
            return true;
        }
        return false;
    }
    
    public void AdvanceGeneration()
    {
        var newCells = new Cell[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var liveNeighbors = CountLiveNeighbors(x, y);
                var isAlive = Cells[x, y].IsAlive;

                newCells[x, y] = isAlive switch
                {
                    true when (liveNeighbors < 2 || liveNeighbors > 3) => new Cell(x, y, false),
                    false when liveNeighbors == 3 => new Cell(x, y, true),
                    _ => new Cell(x, y, isAlive)
                };
            }
        }

        GenerationHistory.Add(Cells);
        Cells = newCells;
    }
    
    public void UndoGeneration()
    {
        if (GenerationHistory.Count <= 0)
        {
            return;
        }

        Cells = GenerationHistory[^1];
        GenerationHistory.RemoveAt(GenerationHistory.Count - 1);
    }
    
    public int GetGenerationCount()
    {
        return GenerationHistory.Count;
    }
    
    public void Initialize()
    {
        Cells = new Cell[Width, Height];
        GenerationHistory = [];   
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Cells[x, y] = new Cell(x, y, false);
            }
        }
    }
    
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
    
    private int CountLiveNeighbors(int x, int y)
    {
        var count = 0;

        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                var nx = x + dx;
                var ny = y + dy;

                if (IsValidPosition(nx, ny) && Cells[nx, ny].IsAlive)
                {
                    count++;
                }
            }
        }

        return count;
    }
}