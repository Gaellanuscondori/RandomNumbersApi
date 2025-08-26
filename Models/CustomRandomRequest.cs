using System.Text.Json.Serialization;

namespace RandomNumbersApi.Models;

public class CustomRandomRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RandomType Type { get; set; }

    public int? Min { get; set; }       // solo number
    public int? Max { get; set; }       // solo number
    public int? Decimals { get; set; } = 2; // solo decimal
    public int? Length { get; set; } = 8;   // solo string
}