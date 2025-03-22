using System;
using System.Text.Json.Serialization;

namespace Conway.Api.Dto;

public record GameStatusDto(int Height, int Width, int[,] Cells, int Ticks)
{
	[JsonConverter(typeof(IntArray2DJsonConverter))]
	public int[,] Cells { get; init; } = Cells;
}
