﻿using Newtonsoft.Json;
using RichillCapital.Binance.Converters;
using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdM;

public interface IBinanceUsdMRestClient 
{
    Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceAccountInformationResponse>> GetAccountInformationAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<BinanceAccountBalanceResponse>>> GetAccountBalancesAsync(CancellationToken cancellationToken = default);
    Task<Result<NewOrderResponse>> NewOrderAsync(NewOrderRequest request, CancellationToken cancellationToken = default);
}

public sealed record NewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; } // BUY or SELL
    public required string Type { get; init; } // MARKET / LIMIT / STOP / TAKE_PROFIT / STOP_MARKET / TAKE_PROFIT_MARKET / TRAILING_STOP_MARKET
    public required decimal Quantity { get; init; }
}

public sealed record NewOrderResponse
{
}

public sealed record BinanceExchangeInfoResponse
{
    public required string Timezone { get; init; }

    [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
    public required DateTimeOffset ServerTime { get; init; }

    public required IEnumerable<object> ExchangeFilters { get; init; }
    public required IEnumerable<BinanceRateLimitResponse> RateLimits { get; init; }
    public required IEnumerable<BinanceAssetResponse> Assets { get; init; }
    public required IEnumerable<BinanceSymbolResponse> Symbols { get; init; }
}

public sealed record BinanceSymbolResponse
{
    public required string Symbol { get; init; }
    public required string Pair { get; init; }
    public required string ContractType { get; init; }

    [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
    public required DateTimeOffset DeliveryDate { get; init; }

    [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
    public required DateTimeOffset OnBoardDate { get; init; }

    public required string Status { get; init; }
    public required decimal MaintMarginPercent { get; init; }
    public required decimal RequiredMarginPercent { get; init; }
    public required string BaseAsset { get; init; }
    public required string QuoteAsset { get; init; }
    public required string MarginAsset { get; init; }
    public required int PricePrecision { get; init; }
    public required int QuantityPrecision { get; init; }
    public required int BaseAssetPrecision { get; init; }
    public required int QuotePrecision { get; init; }
    public required string UnderlyingType { get; init; }
    public required IEnumerable<string> UnderlyingSubType { get; init; }
    public required int SettlePlan { get; init; }
    public required decimal TriggerProtect { get; init; }
    public required IEnumerable<object> Filters { get; init; }
    public required IEnumerable<string> OrderType { get; init; }
    public required IEnumerable<string> TimeInForce { get; init; }
    public required decimal LiquidationFee { get; init; }
    public required decimal MarketTakeBound { get; init; }
}

public sealed record BinanceAssetResponse
{
    public required string Asset { get; init; }
    public required bool MarginAvailable { get; init; }
    public required decimal AutoAssetExchange { get; init; }
}

public sealed record BinanceRateLimitResponse
{
    public required string Interval { get; init; }

    [JsonProperty("intervalNum")]
    public required int IntervalNumber { get; init; }

    public required int Limit { get; init; }

    public required string RateLimitType { get; init; }
}

public sealed record BinanceAccountBalanceResponse
{
    public required string AccountAlias { get; init; }
    public required string Asset { get; init; }
    public required decimal Balance { get; init; }
    public required decimal CrossWalletBalance { get; init; }
    public required decimal CrossPnL { get; init; }
    public required decimal AvailableBalance { get; init; }
    public required decimal MaxWithdrawAmount { get; init; }
    public required bool MarginAvailable { get; init; }

    [JsonConverter(typeof(TimestampToDateTimeOffsetConverter))]
    public required DateTimeOffset UpdateTime { get; init; }
}

public sealed record BinanceAccountInformationResponse
{
    public required decimal TotalInitialMargin { get; init; }
    public required decimal TotalMantMargin { get; init; }
    public required decimal TotalWalletBalance { get; init; }
    public required decimal TotalUnrealizedProfit { get; init; }
    public required decimal TotalMarginBalance { get; init; }
    public required decimal TotalPositionInitialMargin { get; init; }
    public required decimal TotalOpenOrderInitialMargin { get; init; }
    public required decimal TotalCrossWalletBalance { get; init; }
    public required decimal TotalCrossUnPnl { get; init; }
    public required decimal AvailableBalance { get; init; }
    public required decimal MaxWithdrawAmount { get; init; }
    public required IEnumerable<object> Assets { get; init; }
    public required IEnumerable<object> Positions { get; init; }
}