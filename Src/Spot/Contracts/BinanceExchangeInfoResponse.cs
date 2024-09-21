using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace RichillCapital.Binance.Spot.Contracts;

public sealed record BinanceExchangeInfoResponse
{
    [JsonProperty("timezone")]
    public required string Timezone { get; init; }

    [JsonProperty("serverTime")]
    public required long ServerTime { get; init; }

    [JsonProperty("symbols")]
    public required BinanceExchangeInfoSymbolResponse[] Symbols { get; init; }

    // rateLimits

    // exchangeFilters

    // sors
}

public sealed record BinanceExchangeInfoSymbolResponse
{
    [JsonProperty("symbol")]
    public required string Symbol { get; init; }

    [JsonProperty("status")]
    public required string Status { get; init; }

    [JsonProperty("baseAsset")]
    public required string BaseAsset { get; init; }

    [JsonProperty("baseAssetPrecision")]
    public required int BaseAssetPrecision { get; init; }

    [JsonProperty("quoteAsset")]
    public required string QuoteAsset { get; init; }

    [JsonProperty("quoteAssetPrecision")]
    public required int QuoteAssetPrecision { get; init; }

    [JsonProperty("baseCommissionPrecision")]
    public required int BaseCommissionPrecision { get; init; }

    [JsonProperty("quoteCommissionPrecision")]
    public required int QuoteCommissionPrecision { get; init; }

    [JsonProperty("orderTypes")]
    public required string[] OrderTypes { get; init; }

    [JsonProperty("icebergAllowed")]
    public required bool IcebergAllowed { get; init; }

    [JsonProperty("ocoAllowed")]
    public required bool OcoAllowed { get; init; }

    [JsonProperty("quoteOrderQtyMarketAllowed")]
    public required bool QuoteOrderQtyMarketAllowed { get; init; }

    [JsonProperty("allowTrailingStop")]
    public required bool AllowTrailingStop { get; init; }

    [JsonProperty("cancelReplaceAllowed")]
    public required bool CancelReplaceAllowed { get; init; }

    [JsonProperty("isSpotTradingAllowed")]
    public required bool IsSpotTradingAllowed { get; init; }

    [JsonProperty("isMarginTradingAllowed")]
    public required bool IsMarginTradingAllowed { get; init; }

    // filters
    // public required string[] Filters { get; init; }

    // [JsonProperty("permissions")]
    // public required string[] Permissions { get; init; }

    // [JsonProperty("permissionSets")]
    // public required string[] PermissionSets { get; init; }

    [JsonProperty("defaultSelfTradePreventionMode")]
    public required string DefaultSelfTradePreventionMode { get; init; }

    [JsonProperty("allowedSelfTradePreventionModes")]
    public required string[] AllowedSelfTradePreventionModes { get; init; }
}