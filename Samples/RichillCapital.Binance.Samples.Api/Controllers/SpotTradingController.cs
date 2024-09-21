using Microsoft.AspNetCore.Mvc;

using RichillCapital.Binance.Spot;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Samples.Api.Controllers;

[ApiController]
public class SpotTradingController(
    IBinanceSpotRestClient _spotRestClient) :
    BinanceController
{
}
