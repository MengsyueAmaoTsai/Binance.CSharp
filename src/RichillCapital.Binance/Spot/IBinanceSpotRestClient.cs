using RichillCapital.Binance.Abstractions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

public interface IBinanceSpotRestClient : IBinanceRestClient
{
    Task<Result<object>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
}
