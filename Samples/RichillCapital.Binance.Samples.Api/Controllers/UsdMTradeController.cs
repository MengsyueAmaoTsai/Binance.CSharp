using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.UsdM;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class UsdMTradeController(
    IBinanceUsdMRestClient _spotRestClient) :
    BinanceController
{
    [HttpPost("api/usd-m/orders")]
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

        return HandleResult(result);
    }
}

public sealed record UsdMNewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; }
    public required string Type { get; init; }
    public required decimal Quantity { get; init; }
}
