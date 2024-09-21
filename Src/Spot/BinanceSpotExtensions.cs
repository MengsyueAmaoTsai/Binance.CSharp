using Microsoft.Extensions.DependencyInjection;

using RichillCapital.Binance.Authentication;

namespace RichillCapital.Binance.Spot;

public static class BinanceSpotExtensions
{
    public static IServiceCollection AddBinanceSpotRestClient(
        this IServiceCollection services,
        string baseAddress)
    {
        services.AddTransient<BinanceSignatureService>();

        services.AddHttpClient<IBinanceSpotRestClient, BinanceSpotRestClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Clear();
        });

        return services;
    }
}