using System.Text;

using Microsoft.Extensions.Logging;

using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Margin;

internal sealed class BinanceMarginRestClient(
    ILogger<BinanceMarginRestClient> _logger,
    HttpClient _httpClient,
    BinanceSignatureService _signatureService) :
    BinanceRestClient(_logger),
    IBinanceMarginRestClient
{
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

        var path = "sapi/v1/margin/order";
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             path,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response, cancellationToken);
    }
}