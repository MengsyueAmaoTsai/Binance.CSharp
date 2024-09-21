using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdM;

public interface IBinanceUsdMRestClient
{
    Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default);
}