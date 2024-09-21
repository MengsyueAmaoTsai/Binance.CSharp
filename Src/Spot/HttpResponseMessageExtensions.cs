using System.Net;

using RichillCapital.SharedKernel;

namespace RichillCapital.Binance.Spot;

internal static class HttpResponseMessageExtensions
{
    internal static ErrorType GetErrorType(this HttpResponseMessage response) =>
        response switch
        {
            { StatusCode: HttpStatusCode.BadRequest } => ErrorType.Validation,
            { StatusCode: HttpStatusCode.Unauthorized } => ErrorType.Unauthorized,
            { StatusCode: HttpStatusCode.Forbidden } => ErrorType.Forbidden,
            { StatusCode: HttpStatusCode.NotFound } => ErrorType.NotFound,
            { StatusCode: HttpStatusCode.Conflict } => ErrorType.Conflict,
            { StatusCode: HttpStatusCode.InternalServerError } => ErrorType.Unexpected,
            _ => ErrorType.Unexpected,
        };
}