using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RichillCapital.Binance.Shared;

namespace RichillCapital.Binance.Spot;

public static class BinanceSpotExtensions
{
    private static readonly string[] BaseAddresses = [
        "https://api.binance.com",
        "https://api-gcp.binance.com",
        "https://api1.binance.com",
        "https://api2.binance.com",
        "https://api3.binance.com",
        "https://api4.binance.com",
    ];

    private const string SecretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

    public static IServiceCollection AddBinanceSpot(this IServiceCollection services)
    {
        services.TryAddTransient<HttpResponseHandler>();

        services.AddHttpClient<IBinanceSpotRestClient, BinanceSpotRestClient>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("https://api.binance.com");
            client.Timeout = DefaultTimeout;
        });

        return services;
    }
}