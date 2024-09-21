using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdMargined;
using RichillCapital.Binance.Margin;
using RichillCapital.Binance.Shared;
using RichillCapital.Binance.Samples.Api;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseCustomLogger();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Add binance services
builder.Services
    .AddOptions<BinanceOptions>()
    .Bind(builder.Configuration.GetSection("Binance"));

builder.Services.AddBinanceSignatureService();

var binanceOptions = builder.Configuration
    .GetSection("Binance")
    .Get<BinanceOptions>() ??
    throw new ArgumentNullException(nameof(BinanceOptions));

builder.Services.AddBinanceSpotRestClient(binanceOptions.BaseAddresses.Spot);
builder.Services.AddBinanceMarginRestClient(binanceOptions.BaseAddresses.Margin);
builder.Services.AddBinanceUsdMarginedRestClient(binanceOptions.BaseAddresses.UsdMargined);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();


internal sealed record BinanceOptions
{
    public required BaseAddressesOptions BaseAddresses { get; init; }
}

internal sealed record BaseAddressesOptions
{
    public required string Spot { get; init; }
    public required string Margin { get; init; }
    public required string UsdMargined { get; init; }
}