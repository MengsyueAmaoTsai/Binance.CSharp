using Microsoft.AspNetCore.Mvc;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Samples.Api.Controllers;

public abstract class BinanceController : ControllerBase
{
    protected IActionResult HandleResult(Result result) =>
        result.IsFailure ? BadRequest(result) : NoContent();

    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsFailure ? BadRequest(result) : Ok(result.Value);
}