using Newtonsoft.Json;

namespace RichillCapital.Binance.UsdMargined.Contracts;

public sealed record PositionModeResponse
{
    [JsonProperty("dualSidePosition")]
    public required bool DualSidePosition { get; init; }
}