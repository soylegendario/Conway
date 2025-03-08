namespace Conway.Domain;

public sealed class Cell(int x, int y, bool isAlive)
{
    public bool IsAlive { get; private set; } = isAlive;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public void ToggleState()
    {
        IsAlive = !IsAlive;
    }
}