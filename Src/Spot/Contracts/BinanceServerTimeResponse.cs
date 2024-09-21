using Newtonsoft.Json;

namespace RichillCapital.Binance.Spot.Contracts;

public sealed record BinanceServerTimeResponse
{
    [JsonProperty("serverTime")]
    public required long ServerTime { get; init; }
}
