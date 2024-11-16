using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotRestClient(
    HttpClient _httpClient,
    HttpResponseHandler _responseHandler) :
    IBinanceSpotRestClient
{
    public async Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default)
    {
        var path = "/api/v3/ping";

        var response = await _httpClient.GetAsync(path, cancellationToken);

        return await _responseHandler.HandleResponseAsync<object>(response);
    }
}