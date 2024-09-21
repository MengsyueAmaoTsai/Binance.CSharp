using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.UsdMargined;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class UsdMarginedTradeController(
    IBinanceUsdMarginedRestClient _restClient) :
    BinanceController
{
    [HttpPost("api/usd-m/orders")]
    public async Task<IActionResult> NewOrderAsync(
        [FromBody] UsdMarginedNewOrderRequest request,
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

public sealed record UsdMarginedNewOrderRequest
{
    public required string Symbol { get; init; }
    public required string Side { get; init; }
    public required string Type { get; init; }
    public required decimal Quantity { get; init; }
}
