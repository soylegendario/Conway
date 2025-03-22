using System.Text.Json.Serialization;

namespace Conway.Domain;

public record GameStatus(int Height, int Width, int[,] Cells, int Ticks);
