using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.UsdM;

public static class BinanceUsdMExtensions
{
    private const string BaseAddress = "https://fapi.binance.com";
    private const string BaseAddressTestNet = "https://testnet.binancefuture.com";

    private const string ApiKey = "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92";
    private const string SecretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

    public static IServiceCollection AddBinanceUsdM(this IServiceCollection services)
    {
        services.AddHttpClient<IBinanceUsdMRestClient, BinanceUsdMRestClient>(client =>
        {
            client.BaseAddress = new Uri(BaseAddress);
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", ApiKey);
        });

        return services;
    }
}
