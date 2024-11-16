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

        _logger.LogInformation("Response content: {content}", content);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var error = await httpResponse.ReadAsAsync<BinanceErrorResponse>(cancellationToken)
                .Match(
                    res => Error.Unexpected(res.Code.ToString(), res.Message),
                    error => error);

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