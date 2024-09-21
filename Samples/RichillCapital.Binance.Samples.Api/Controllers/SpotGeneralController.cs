using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Spot;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class SpotGeneralController(
    IBinanceSpotRestClient _spotRestClient) :
    BinanceController
{
    [HttpGet("api/spot/ping")]
    public async Task<IActionResult> PingAsync(CancellationToken cancellationToken = default)
    {
        var result = await _spotRestClient.PingAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("api/spot/server-time")]
    public async Task<IActionResult> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var result = await _spotRestClient.GetServerTimeAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("api/spot/exchange-info")]
    public async Task<IActionResult> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var result = await _spotRestClient.GetExchangeInfoAsync(cancellationToken);
        return HandleResult(result);
    }
}
