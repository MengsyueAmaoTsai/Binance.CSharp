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
builder.Services.AddBinanceSignatureService();
builder.Services.AddBinanceSpotRestClient("https://api.binance.com");
builder.Services.AddBinanceMarginRestClient("https://api.binance.com");
builder.Services.AddBinanceUsdMarginedRestClient("https://fapi.binance.com");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
