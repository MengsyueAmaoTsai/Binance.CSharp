using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Logging;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotSignatureService(
    ILogger<BinanceSpotSignatureService> _logger)
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