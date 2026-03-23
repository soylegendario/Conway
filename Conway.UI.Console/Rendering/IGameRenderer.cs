using Conway.Domain;

namespace Conway.UI.Console.Rendering;

public interface IGameRenderer
{
    /// <summary>
    /// Inicia el bucle de renderizado bloqueante en la terminal.
    /// </summary>
    /// <returns>True si el usuario quiere volver al inicio, False si quiere salir del programa.</returns>
    Task<bool> StartRenderLoopAsync(IGameGrid grid, string gameId, int tickDelayMs);
}
