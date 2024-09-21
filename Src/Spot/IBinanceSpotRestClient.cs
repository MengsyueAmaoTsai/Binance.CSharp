using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

public interface IBinanceSpotRestClient
{
    Task<Result> PingAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default);
}