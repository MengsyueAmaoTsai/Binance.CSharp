using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.UsdMargined;

namespace RichillCapital.Binance.Samples.Api.Controllers.UsdMargined;

public class UsdMarginedAccountController(
    IBinanceUsdMarginedRestClient _restClient) :
    BinanceController
{
    [HttpGet("api/usd-m/balance")]
    public async Task<IActionResult> NewOrderAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await _restClient.GetAccountBalanceAsync(cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("api/usd-m/assets-mode")]
    public async Task<IActionResult> GetAssetsModeAsync(CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}
