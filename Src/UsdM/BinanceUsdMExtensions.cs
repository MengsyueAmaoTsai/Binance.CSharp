using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.UsdM;

public static class BinanceUsdMExtensions
{
    public static IServiceCollection AddBinanceUsdMRestClient(
        this IServiceCollection services,
        string baseAddress)
    {
        // services.AddTransient<BinanceSpotSignatureService>();

        services.AddHttpClient<IBinanceUsdMRestClient, BinanceUsdMRestClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Clear();
        });

        return services;
    }
}