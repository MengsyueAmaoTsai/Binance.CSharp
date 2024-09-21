using System.Text;

using Microsoft.Extensions.Logging;

using RichillCapital.Binance.Shared;
using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotRestClient(
    ILogger<BinanceSpotRestClient> _logger,
    HttpClient _httpClient,
    BinanceSignatureService _signatureService) :
    BinanceRestClient(_logger),
    IBinanceSpotRestClient
{
    public async Task<Result> PingAsync(CancellationToken cancellationToken = default)
    {
        var path = BinanceSpotApiRoutes.General.Ping;
        var response = await _httpClient.GetAsync(path);

        return await HandleResponseAsync(response, cancellationToken);
    }

    public async Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BinanceSpotApiRoutes.General.ServerTime);
        return await HandleResponseAsync<BinanceServerTimeResponse>(response, cancellationToken);
    }

    public async Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BinanceSpotApiRoutes.General.ExchangeInfo);
        return await HandleResponseAsync<BinanceExchangeInfoResponse>(response, cancellationToken);
    }

    public async Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default)
    {
        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}";
        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(SecretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", ApiKey);

        var path = BinanceSpotApiRoutes.Trading.NewOrder;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             BinanceSpotApiRoutes.Trading.NewOrder,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response, cancellationToken);
    }

    public async Task<Result> TestNewOrderAsync(string symbol, string side, string type, decimal quantity, CancellationToken cancellationToken = default)
    {
        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}";
        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(SecretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", ApiKey);

        var path = BinanceSpotApiRoutes.Trading.NewOrder;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             BinanceSpotApiRoutes.Trading.TestNewOrder,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response, cancellationToken);
    }
}
