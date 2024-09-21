using Microsoft.Extensions.Logging;

using RichillCapital.Binance.Extensions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance;

internal abstract class BinanceRestClient(
    ILogger<BinanceRestClient> _logger)
{
    protected const int RecvWindow = 60000;
    protected const string ApiKey = "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92";
    protected const string SecretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

    protected async Task<Result<TBinanceResponse>> HandleResponseAsync<TBinanceResponse>(
        HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        var uri = response.RequestMessage?.RequestUri;
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Request {path} failed. Status: {status}. Content: {content}",
                uri,
                response.StatusCode,
                responseContent);

            var error = await response.ReadAsErrorAsync(cancellationToken);
            _logger.LogError("Transformed {error}", error);

            return Result<TBinanceResponse>.Failure(error);
        }

        var binanceResponse = await response.ReadAsAsync<TBinanceResponse>();
        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}, Content: {binanceResponse}");

        return Result<TBinanceResponse>.With(binanceResponse!);
    }

    protected async Task<Result> HandleResponseAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        var uri = response.RequestMessage?.RequestUri;
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Request {path} failed. Status: {status}. Content: {content}",
                uri,
                response.StatusCode,
                responseContent);

            var error = await response.ReadAsErrorAsync(cancellationToken);
            _logger.LogError("Transformed {error}", error);

            return Result.Failure(error);
        }

        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}, {responseContent}");

        return Result.Success;
    }
}