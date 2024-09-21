using System.Net.Http.Json;

using Microsoft.Extensions.Logging;

using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotRestClient(
    ILogger<BinanceSpotRestClient> _logger,
    HttpClient _httpClient) :
    IBinanceSpotRestClient
{
    public async Task<Result> PingAsync(CancellationToken cancellationToken = default)
    {
        var path = BinanceSpotApiRoutes.General.Ping;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.GetAsync(path);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request {path} failed. Status: {status}", path, response.StatusCode);

            return Result.Failure(Error.Unexpected($"Request {path} failed. Status: {response.StatusCode}"));
        }

        _logger.LogInformation($"Success send request: {path}. Status: {response.StatusCode}");

        return Result.Success;
    }

    public async Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var path = BinanceSpotApiRoutes.General.ServerTime;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.GetAsync(path);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request {path} failed. Status: {status}", path, response.StatusCode);

            return Result<BinanceServerTimeResponse>.Failure(Error.Unexpected($"Request {path} failed. Status: {response.StatusCode}"));
        }

        _logger.LogInformation($"Success send request: {path}. Status: {response.StatusCode}");
        var serverTime = await response.Content.ReadFromJsonAsync<BinanceServerTimeResponse>();

        return Result<BinanceServerTimeResponse>.With(serverTime!);
    }

    public async Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var path = BinanceSpotApiRoutes.General.ExchangeInfo;

        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.GetAsync(path);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Request {path} failed. Status: {status}", path, response.StatusCode);

            return Result<BinanceExchangeInfoResponse>.Failure(Error.Unexpected($"Request {path} failed. Status: {response.StatusCode}"));
        }

        _logger.LogInformation($"Success send request: {path}. Status: {response.StatusCode}");
        var exchangeInfo = await response.Content.ReadFromJsonAsync<BinanceExchangeInfoResponse>();

        return Result<BinanceExchangeInfoResponse>.With(exchangeInfo!);
    }
}
