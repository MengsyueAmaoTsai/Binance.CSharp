using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.UsdM;

public static class BinanceUsdMExtensions
{
    private const string BaseAddress = "https://fapi.binance.com";
    private const string BaseAddressTestNet = "https://testnet.binancefuture.com";

    public static IServiceCollection AddBinanceUsdM(this IServiceCollection services)
    {
        services.AddHttpClient<IBinanceUsdMRestClient, BinanceUsdMRestClient>(client =>
        {
            client.BaseAddress = new Uri(BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
