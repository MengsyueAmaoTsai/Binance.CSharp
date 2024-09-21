using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Spot;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class SpotGeneralController(
    IBinanceSpotRestClient _spotRestClient) :
    ControllerBase
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

    private IActionResult HandleResult(Result result) =>
        result.IsFailure ? BadRequest(result) : NoContent();

    private IActionResult HandleResult<T>(Result<T> result) =>
        result.IsFailure ? BadRequest(result) : Ok(result.Value);
}
