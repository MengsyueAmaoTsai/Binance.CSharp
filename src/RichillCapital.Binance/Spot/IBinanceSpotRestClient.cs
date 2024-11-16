using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

public interface IBinanceSpotRestClient
{
    Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default);
    Task<Result<object>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
}
