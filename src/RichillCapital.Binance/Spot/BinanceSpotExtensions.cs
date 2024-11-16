using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Spot;

public static class BinanceSpotExtensions
{
    public static IServiceCollection AddBinanceSpot(this IServiceCollection services)
    {
        services.AddHttpClient<IBinanceSpotRestClient, BinanceSpotRestClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.binance.com");
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}