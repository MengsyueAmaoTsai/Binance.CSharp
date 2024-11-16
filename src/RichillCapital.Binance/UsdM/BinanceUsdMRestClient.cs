using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;
using System.Security.Cryptography;
using System.Text;

namespace RichillCapital.Binance.UsdM;

internal sealed class BinanceUsdMRestClient(
    ILogger<BinanceUsdMRestClient> _logger,
    HttpClient _httpClient) : 
    IBinanceUsdMRestClient
{
    public async Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/time";
        var httpResponse = await _httpClient.GetAsync(path);

        return await HandleResponseAsync<BinanceServerTimeResponse>(httpResponse, cancellationToken);
    }

    public async Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/ping";

        var response = await _httpClient.GetAsync(path);

        return await HandleResponseAsync<object>(response, cancellationToken);
    }

    public async Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/exchangeInfo";
        var httpResponse = await _httpClient.GetAsync(path);

        return await HandleResponseAsync<BinanceExchangeInfoResponse>(httpResponse, cancellationToken);
    }

    public async Task<Result<BinanceAccountInformationResponse>> GetAccountInformationAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v3/account";
        
        var queryString = $"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var signature = CreateSignature(queryString);
        queryString += $"&signature={signature}";

        var response = await _httpClient.GetAsync($"{path}?{queryString}");

        return await HandleResponseAsync<BinanceAccountInformationResponse>(response, cancellationToken);
    }

    public async Task<Result<IEnumerable<BinanceAccountBalanceResponse>>> GetAccountBalancesAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v3/balance";

        var queryString = $"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var signature = CreateSignature(queryString);
        queryString += $"&signature={signature}";

        var response = await _httpClient.GetAsync($"{path}?{queryString}");

        return await HandleResponseAsync<IEnumerable<BinanceAccountBalanceResponse>>(response, cancellationToken);
    }

    private async Task<Result<TResponse>> HandleResponseAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogInformation("Response content: {content}", content);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var error = DeserializeAs<BinanceErrorResponse>(content)
                .Match(
                    res => Error.Unexpected(res.Code.ToString(), res.Message),
                    error => error);
            
            return Result<TResponse>.Failure(error);
        }

        return DeserializeAs<TResponse>(content);
    }

    private Result<TResponse> DeserializeAs<TResponse>(string content)
    {
        try
        {
            return Result<TResponse>.With(JsonConvert.DeserializeObject<TResponse>(content)!);
        }
        catch (JsonException ex)
        {
            _logger.LogError("Failed to deserialize response: {Content}", content);
            return Result<TResponse>.Failure(Error.Unexpected("0", $"Failed to deserialize response. {ex}"));
        }
    }

    private static string CreateSignature(string queryString)
    {
        var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var secretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryString));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
