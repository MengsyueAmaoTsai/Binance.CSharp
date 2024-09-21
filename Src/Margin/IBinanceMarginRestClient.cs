using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Margin;

public interface IBinanceMarginRestClient
{
    Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default);
}