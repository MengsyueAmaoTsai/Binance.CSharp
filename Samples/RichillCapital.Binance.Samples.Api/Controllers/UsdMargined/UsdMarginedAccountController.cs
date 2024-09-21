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

    [HttpGet("api/usd-m/account")]
    public async Task<IActionResult> GetAccountInformationAsync(CancellationToken cancellationToken = default)
    {
        var result = await _restClient.GetAccountInformationAsync(cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("api/usd-m/assets-mode")]
    public async Task<IActionResult> GetAssetsModeAsync(CancellationToken cancellationToken = default)
    {
        return Ok();
    }

    [HttpGet("api/usd-m/account-configuration")]
    public async Task<IActionResult> GetAccountConfigurationAsync(CancellationToken cancellationToken = default)
    {
        var result = await _restClient.GetAccountConfigurationAsync(cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("api/usd-m/position-mode")]
    public async Task<IActionResult> GetPositionModeAsync(CancellationToken cancellationToken = default)
    {
        var result = await _restClient.GetPositionModeAsync(cancellationToken);

        return HandleResult(result);
    }
}
