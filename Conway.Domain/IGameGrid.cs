namespace Conway.Domain;

public interface IGameGrid
{
    Task<string> NewGame(int width, int height);
    GameStatus? GetWorld(string gameId);
    bool ToggleCellState(string gameId, int x, int y);
    bool AdvanceGeneration(string gameId);
    bool Shuffle(string gameId);
    bool UndoGeneration(string gameId);
}