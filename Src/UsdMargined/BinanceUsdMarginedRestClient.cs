using System.Text;

using Microsoft.Extensions.Logging;

using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdMargined;

internal sealed class BinanceUsdMarginedRestClient(
    ILogger<BinanceUsdMarginedRestClient> _logger,
    HttpClient _httpClient,
    BinanceSignatureService _signatureService) :
    BinanceRestClient(_logger),
    IBinanceUsdMarginedRestClient
{
    private enum OrderResponseType
    {
        Ack = 1,
        Result = 2,
    }

    private enum PositionSide
    {
        Long = 1,
        Short = 2,
    }

    public async Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default)
    {
        var clientOrderId = Guid.NewGuid();
        var responseType = OrderResponseType.Ack;
        var positionSide = (side == "BUY" ? PositionSide.Long : PositionSide.Short).ToString().ToUpperInvariant();

        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}&newClientOrderId={clientOrderId}&newOrderRespType={responseType}&positionSide={positionSide}";

        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(SecretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", ApiKey);
        _logger.LogInformation("Apply API key: {apiKey}", ApiKey);

        var path = "fapi/v1/order";
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             path,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response, cancellationToken);
    }
}