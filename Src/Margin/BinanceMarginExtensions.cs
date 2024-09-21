using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Margin;

public static class BinanceMarginExtensions
{
    public static IServiceCollection AddBinanceMarginRestClient(
        this IServiceCollection services,
        string baseAddress)
    {
        services.AddHttpClient<IBinanceMarginRestClient, BinanceMarginRestClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Clear();
        });

        return services;
    }
}