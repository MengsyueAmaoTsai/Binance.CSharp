using RichillCapital.Binance.UsdMargined.Contracts;
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

    Task<Result<BinanceAccountBalanceResponse>> GetAccountBalanceAsync(CancellationToken cancellationToken = default);
    Task<Result<PositionModeResponse>> GetPositionModeAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceAccountConfigurationResponse>> GetAccountConfigurationAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceAccountInformationResponse>> GetAccountInformationAsync(CancellationToken cancellationToken = default);
}