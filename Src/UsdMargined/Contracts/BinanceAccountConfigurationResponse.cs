using Newtonsoft.Json;

namespace RichillCapital.Binance.UsdMargined.Contracts;

public sealed record BinanceAccountConfigurationResponse
{
    [JsonProperty("feeTier")]
    public required int FeeTier { get; init; }

    [JsonProperty("canTrade")]
    public required bool CanTrade { get; init; }

    [JsonProperty("canDeposit")]
    public required bool CanDeposit { get; init; }

    [JsonProperty("canWithdraw")]
    public required bool CanWithdraw { get; init; }

    [JsonProperty("dualSidePosition")]
    public required bool DualSidePosition { get; init; }

    [JsonProperty("updateTime")]
    public required long UpdateTime { get; init; }

    [JsonProperty("multiAssetsMargin")]
    public required bool MultiAssetsMargin { get; init; }

    [JsonProperty("tradeGroupId")]
    public required int TradeGroupId { get; init; }
}