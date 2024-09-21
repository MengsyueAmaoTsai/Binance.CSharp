using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.UsdMargined;

public static class BinanceUsdMarginedExtensions
{
    public static IServiceCollection AddBinanceUsdMarginedRestClient(
        this IServiceCollection services,
        string baseAddress)
    {
        services.AddHttpClient<IBinanceUsdMarginedRestClient, BinanceUsdMarginedRestClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Clear();
        });

        return services;
    }
}