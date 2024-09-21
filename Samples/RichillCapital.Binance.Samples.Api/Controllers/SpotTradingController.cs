using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Spot;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class SpotTradingController(
    IBinanceSpotRestClient _restClient) :
    BinanceController
{
    [HttpPost("api/spot/orders")]
    public async Task<IActionResult> NewOrderAsync(
        [FromBody] NewOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _restClient.NewOrderAsync(
            request.Symbol,
            request.Side,
            request.Type,
            request.Quantity,
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPost("api/spot/orders/test")]
    public async Task<IActionResult> TestNewOrderAsync(
        [FromBody] NewOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _restClient.TestNewOrderAsync(
            request.Symbol,
            request.Side,
            request.Type,
            request.Quantity,
            cancellationToken);

        return HandleResult(result);
    }
}

public sealed record NewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; }
    public required string Type { get; init; }
    public required decimal Quantity { get; init; }
}
