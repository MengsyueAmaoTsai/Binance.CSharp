using Microsoft.AspNetCore.Mvc;

using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public abstract class BinanceController : ControllerBase
{
    protected IActionResult HandleResult(Result result) =>
        result.IsFailure ? 
            BadRequest(new
            {
                ErrorType = result.Error.Type,
                ErrorCode = result.Error.Code,
                result.Error.Message,
            }) : 
            NoContent();

    protected IActionResult HandleResult<T>(Result<T> result) =>
        result.IsFailure ? 
            BadRequest(new
            {
                ErrorType = result.Error.Type,
                ErrorCode = result.Error.Code,
                result.Error.Message,
            }) : 
            Ok(result.Value);
}