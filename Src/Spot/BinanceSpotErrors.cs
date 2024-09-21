using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel;

namespace RichillCapital.Binance.Spot.Errors;

internal static class BinanceSpotErrors
{
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
            -2010 => "NewOrderRejected",
            _ => throw new NotImplementedException($"{nameof(binanceErrorCode)} for {binanceErrorCode} is not defined."),
        };

        return $"BinanceSpot.{suffix}";
    }
}