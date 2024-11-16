using RichillCapital.Binance.Converters;
using System.Text.Json.Serialization;

namespace RichillCapital.Binance.Shared;

public sealed record BinanceServerTimeResponse
{
    [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
    public required DateTimeOffset ServerTime { get; init; }
}