using Conway.Domain;
using Conway.UI.Console.Rendering;

IGameGrid grid = new GameGrid();
string gameId = await grid.NewGame(50, 50);
grid.Shuffle(gameId);

IGameRenderer renderer = new SpectreLiveRenderer();
await renderer.StartRenderLoopAsync(grid, gameId, 100);