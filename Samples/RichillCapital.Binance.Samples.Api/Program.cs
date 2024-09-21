using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdMargined;
using RichillCapital.Binance.Margin;
using RichillCapital.Binance.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add binance services
builder.Services.AddBinanceSignatureService();
builder.Services.AddBinanceSpotRestClient("https://api.binance.com");
builder.Services.AddBinanceMarginRestClient("https://api.binance.com");
builder.Services.AddBinanceUsdMarginedRestClient("https://fapi.binance.com");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
