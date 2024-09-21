using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RichillCapital.Binance.Shared;

internal sealed class BinanceSignatureService(
    ILogger<BinanceSignatureService> _logger)
{
    internal string Sign(string secretKey, string queryString)
    {
        _logger.LogInformation(
            "Signing with secret key: {secretKey} and query string: {queryString}",
            secretKey,
            queryString);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

        var signature = BitConverter
            .ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(queryString)))
            .Replace("-", string.Empty)
            .ToLower();

        _logger.LogInformation(
            "\n========================= Signature =========================\n{signature}\n========================= Signature =========================\n",
            signature);

        return signature;
    }
}

public static partial class BinanceExtensions
{
    public static IServiceCollection AddBinanceSignatureService(this IServiceCollection services)
    {
        services.AddTransient<BinanceSignatureService>();

        return services;
    }
}