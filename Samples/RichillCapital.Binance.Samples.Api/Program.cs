using RichillCapital.Binance.Spot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add binance services
builder.Services.AddBinanceSpotRestClient("https://api.binance.com");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
