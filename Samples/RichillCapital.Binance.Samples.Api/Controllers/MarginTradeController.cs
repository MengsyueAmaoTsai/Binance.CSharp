using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Margin;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class MarginTradeController(
    IBinanceMarginRestClient _restClient) :
    BinanceController
{
    [HttpPost("api/margin/orders")]
    public async Task<IActionResult> NewOrderAsync(
        [FromBody] MarginNewOrderRequest request,
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
}

public sealed record MarginNewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; }
    public required string Type { get; init; }
    public required decimal Quantity { get; init; }
}
