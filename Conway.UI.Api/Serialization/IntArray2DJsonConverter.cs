using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Conway.Api.Serialization;

public class IntArray2DJsonConverter : JsonConverter<int[,]>
{
    public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonArray = JsonSerializer.Deserialize<int[][]>(ref reader, options);
        if (jsonArray == null || jsonArray.Length == 0)
        {
            return new int[0, 0];
        }

        int height = jsonArray.Length;
        int width = jsonArray[0].Length;
        var result = new int[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                result[i, j] = jsonArray[i][j];
            }
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options)
    {
        int height = value.GetLength(0);
        int width = value.GetLength(1);
        writer.WriteStartArray();

        for (int i = 0; i < height; i++)
        {
            writer.WriteStartArray();
            for (int j = 0; j < width; j++)
            {
                writer.WriteNumberValue(value[i, j]);
            }
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
