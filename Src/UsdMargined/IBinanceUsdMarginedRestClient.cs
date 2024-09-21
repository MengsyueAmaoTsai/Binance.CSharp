using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdMargined;

public interface IBinanceUsdMarginedRestClient
{
    Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default);
}