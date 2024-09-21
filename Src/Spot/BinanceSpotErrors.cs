using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel;

namespace RichillCapital.Binance.Spot.Errors;

internal static class BinanceSpotErrors
{
    private const string ErrorCodePrefix = "BinanceSpot";

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
            // SPOT
            -2010 => "NewOrderRejected",

            // USD-M
            -2019 => "MarginNotSufficient",

            // Margin
            -3003 => "NoOpenedMarginAccount",

            _ => throw new NotImplementedException($"{nameof(binanceErrorCode)} for {binanceErrorCode} is not defined."),
        };

        return $"{ErrorCodePrefix}.{suffix}";
    }
}