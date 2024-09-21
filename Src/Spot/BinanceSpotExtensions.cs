using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Spot;

public static class BinanceSpotExtensions
{
    public static IServiceCollection AddBinanceSpotRestClient(
        this IServiceCollection services,
        string baseAddress)
    {
        services.AddHttpClient<IBinanceSpotRestClient, BinanceSpotRestClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Clear();
        });

        return services;
    }
}