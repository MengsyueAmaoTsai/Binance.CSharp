using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Shared;

public static class BinanceExtensions
{
    public static IServiceCollection AddBinanceSignatureService(this IServiceCollection services)
    {
        services.AddTransient<BinanceSignatureService>();

        return services;
    }
}