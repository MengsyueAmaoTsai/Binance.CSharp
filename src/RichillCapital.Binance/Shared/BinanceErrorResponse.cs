using Newtonsoft.Json;

namespace RichillCapital.Binance.Shared;

public sealed record BinanceErrorResponse
{
    public required int Code { get; init; }

    [JsonProperty("msg")]
    public required string Message { get; init; }
}
