using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotRestClient(
    HttpClient _httpClient) :
    IBinanceSpotRestClient
{
    public Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}