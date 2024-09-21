using Newtonsoft.Json;

namespace RichillCapital.Binance.UsdMargined.Contracts;

public sealed record BinanceAccountInformationResponse
{
    [JsonProperty("totalInitialMargin")]
    public required decimal TotalInitialMargin { get; init; }
    [JsonProperty("totalMaintMargin")]
    public required decimal TotalMaintMargin { get; init; }

    [JsonProperty("totalWalletBalance")]
    public required decimal TotalWalletBalance { get; init; }

    [JsonProperty("totalUnrealizedProfit")]
    public required decimal TotalUnrealizedProfit { get; init; }

    [JsonProperty("totalMarginBalance")]
    public required decimal TotalMarginBalance { get; init; }

    [JsonProperty("totalPositionInitialMargin")]
    public required decimal TotalPositionInitialMargin { get; init; }

    [JsonProperty("totalOpenOrderInitialMargin")]
    public required decimal TotalOpenOrderInitialMargin { get; init; }

    [JsonProperty("totalCrossWalletBalance")]
    public required decimal TotalCrossWalletBalance { get; init; }

    [JsonProperty("totalCrossUnPnl")]
    public required decimal TotalCrossUnPnl { get; init; }

    [JsonProperty("availableBalance")]
    public required decimal AvailableBalance { get; init; }

    [JsonProperty("maxWithdrawAmount")]
    public required decimal MaxWithdrawAmount { get; init; }
}