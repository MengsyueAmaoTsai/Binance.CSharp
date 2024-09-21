using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel;

namespace RichillCapital.Binance.Spot.Errors;

internal static class BinanceSpotErrors
{
    private const string ErrorCodePrefix = "Binance";

    internal static Error Create(ErrorType type, BinanceErrorResponse response) =>
        Create(type, ConvertErrorCode(response.Code), response.Message);

    private static Error Create(ErrorType type, string code, string message) =>
        type switch
        {
            ErrorType.Validation => Error.Invalid(code, message),
            ErrorType.Unauthorized => Error.Unauthorized(code, message),
            ErrorType.Forbidden => Error.Forbidden(code, message),
            ErrorType.NotFound => Error.NotFound(code, message),
            ErrorType.Conflict => Error.Conflict(code, message),
            ErrorType.Unexpected => Error.Unexpected(code, message),
            _ => Error.Unexpected(code, message)
        };

    private static string ConvertErrorCode(int binanceErrorCode)
    {
        var suffix = binanceErrorCode switch
        {
            -1000 => "Unknown",

            // SPOT
            -2010 => "NewOrderRejected",

            // USD-M
            -1102 => "MandatoryParamEmptyOrMalformed",

            -1115 => "InvalidTimeInForce",
            -1116 => "InvalidOrderType",
            -1117 => "InvalidSide",
            -1121 => "BadSymbol",
            -2015 => "RejectedMbxKey",
            -2019 => "MarginNotSufficient",
            -2020 => "UnableToFill",
            -2021 => "OrderWouldImmediatelyTrigger",
            -2022 => "ReduceOnlyReject",
            -2023 => "UserInLiquidation",
            -2024 => "PositionNotSufficient",
            -2025 => "MaxOpenOrderExceeded",
            -2026 => "ReduceOnlyOrderTypeNotSupported",
            -2027 => "MaxLeverageRatio",
            -2028 => "MinLeverageRatio",

            -4003 => "QuantityLessThanZero",

            // Margin
            -3003 => "NoOpenedMarginAccount",

            _ => throw new NotImplementedException($"{nameof(binanceErrorCode)} for {binanceErrorCode} is not defined."),
        };

        return $"{ErrorCodePrefix}.{suffix}";
    }
}