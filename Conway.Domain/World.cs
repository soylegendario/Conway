namespace Conway.Domain;

public sealed class World
{
    private const int DefaultMaxHistorySize = 100;

    private readonly Cell[] _bufferA;
    private readonly Cell[] _bufferB;
    private readonly int _maxHistorySize;
    private bool _isBufferAActive;

    public int Width { get; }
    public int Height { get; }
    public List<bool[]> GenerationHistory { get; private set; } = [];

    private Cell[] CurrentBuffer => _isBufferAActive ? _bufferA : _bufferB;
    private Cell[] NextBuffer => _isBufferAActive ? _bufferB : _bufferA;

    public World(int width, int height, int maxHistorySize = DefaultMaxHistorySize)
    {
        Width = width;
        Height = height;
        _maxHistorySize = maxHistorySize;
        _bufferA = new Cell[width * height];
        _bufferB = new Cell[width * height];
        _isBufferAActive = true;
    }

    public Cell GetCell(int x, int y) => CurrentBuffer[y * Width + x];

    public bool ToggleCellState(int x, int y)
    {
        if (!IsValidPosition(x, y)) return false;

        var idx = y * Width + x;
        var buf = CurrentBuffer;
        buf[idx] = new Cell(!buf[idx].IsAlive);
        return true;
    }

    public void AdvanceGeneration()
    {
        var current = CurrentBuffer;
        var next = NextBuffer;

        var snapshot = new bool[Width * Height];
        for (var i = 0; i < current.Length; i++)
            snapshot[i] = current[i].IsAlive;

        Parallel.For(0, Height, y =>
        {
            for (var x = 0; x < Width; x++)
            {
                var liveNeighbors = CountLiveNeighbors(current, x, y);
                var isAlive = current[y * Width + x].IsAlive;
                next[y * Width + x] = new Cell(isAlive switch
                {
                    true when liveNeighbors < 2 || liveNeighbors > 3 => false,
                    false when liveNeighbors == 3 => true,
                    _ => isAlive
                });
            }
        });

        _isBufferAActive = !_isBufferAActive;

        if (GenerationHistory.Count >= _maxHistorySize)
            GenerationHistory.RemoveAt(0);
        GenerationHistory.Add(snapshot);
    }

    public void UndoGeneration()
    {
        if (GenerationHistory.Count <= 0) return;

        var snapshot = GenerationHistory[^1];
        GenerationHistory.RemoveAt(GenerationHistory.Count - 1);

        var buf = CurrentBuffer;
        for (var i = 0; i < snapshot.Length; i++)
            buf[i] = new Cell(snapshot[i]);
    }

    public int GetGenerationCount() => GenerationHistory.Count;

    public void Initialize()
    {
        Array.Clear(_bufferA);
        Array.Clear(_bufferB);
        _isBufferAActive = true;
        GenerationHistory = [];
    }

    private bool IsValidPosition(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    private int CountLiveNeighbors(Cell[] buffer, int x, int y)
    {
        var count = 0;
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                var nx = x + dx;
                var ny = y + dy;
                if (IsValidPosition(nx, ny) && buffer[ny * Width + nx].IsAlive)
                    count++;
            }
        }
        return count;
    }
}
