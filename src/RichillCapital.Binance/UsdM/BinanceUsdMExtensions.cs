using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RichillCapital.Binance.Shared;

namespace RichillCapital.Binance.UsdM;

public static class BinanceUsdMExtensions
{
    private const string BaseAddressTestNet = "https://testnet.binancefuture.com";
    private const string SecretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

    public static IServiceCollection AddBinanceUsdM(this IServiceCollection services)
    {
        services.TryAddTransient<HttpResponseHandler>();

        services.AddHttpClient<IBinanceUsdMRestClient, BinanceUsdMRestClient>((serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("https://fapi.binance.com");
            client.Timeout = DefaultTimeout;
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92");
        });

        return services;
    }
}
