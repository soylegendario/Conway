using Conway.Domain;

namespace Conway.UI.Console.Rendering;

public interface IGameRenderer
{
    /// <summary>
    /// Inicia el bucle de renderizado bloqueante en la terminal.
    /// </summary>
    Task StartRenderLoopAsync(IGameGrid grid, string gameId, int tickDelayMs);
}
