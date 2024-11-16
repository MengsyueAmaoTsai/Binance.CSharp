using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Shared;

internal sealed class HttpResponseHandler(
    ILogger<HttpResponseHandler> _logger)
{
    internal async Task<Result<TResponse>> HandleResponseAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Handling response content: {content}", content);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var errorResponseResult = await httpResponse.ReadAsAsync<BinanceErrorResponse>(cancellationToken);
            
            if (errorResponseResult.IsFailure)
            {
                return Result<TResponse>.Failure(errorResponseResult.Error);
            }

            var response = errorResponseResult.Value;

            var error = Error.Unexpected(
                response.Code.ToString(), 
                response.Message);

            return Result<TResponse>.Failure(error);
        }

        return await httpResponse.ReadAsAsync<TResponse>(cancellationToken);
    }
}

internal static class HttpResponseMessageExtensions
{
    internal static async Task<Result<T>> ReadAsAsync<T>(
        this HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            return Result<T>.With(JsonConvert.DeserializeObject<T>(content)!);
        }
        catch (JsonException ex)
        {
            return Result<T>.Failure(Error.Unexpected($"Failed to deserialize response. {ex}"));
        }
    }
}

