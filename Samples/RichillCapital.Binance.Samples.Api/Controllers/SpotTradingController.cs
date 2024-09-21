using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Spot;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class SpotTradingController(
    IBinanceSpotRestClient _spotRestClient) :
    BinanceController
{
    [HttpPost("api/spot/orders")]
    public async Task<IActionResult> NewOrderAsync(
        [FromBody] NewOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _spotRestClient.NewOrderAsync(
            request.Symbol,
            request.Side,
            request.Type,
            request.Quantity,
            cancellationToken);

        return Ok();
    }
}

public sealed record NewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; }
    public required string Type { get; init; }
    public required decimal Quantity { get; init; }
}
