using Newtonsoft.Json;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdM;

public interface IBinanceUsdMRestClient
{
    Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default);
    Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default);
}

public sealed record BinanceServerTimeResponse
{
    public required DateTimeOffset ServerTime { get; init; }
}

public sealed record BinanceErrorResponse
{
    public required int Code { get; init; }

    [JsonProperty("msg")]
    public required string Message { get; init; }
}