using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Authentication;

public static class BinanceExtensions
{
    public static IServiceCollection AddBinanceSignatureService(this IServiceCollection services)
    {
        services.AddTransient<BinanceSignatureService>();

        return services;
    }
}