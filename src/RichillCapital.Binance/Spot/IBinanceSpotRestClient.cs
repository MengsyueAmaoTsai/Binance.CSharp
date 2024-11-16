using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

public interface IBinanceSpotRestClient
{
    Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default);
}
