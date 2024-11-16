using RichillCapital.Binance.Shared;
using RichillCapital.SharedKernel.Monads;
using System.Security.Cryptography;
using System.Text;

namespace RichillCapital.Binance.UsdM;

internal sealed class BinanceUsdMRestClient(
    HttpClient _httpClient,
    HttpResponseHandler _responseHandler) :
    IBinanceUsdMRestClient
{
    public async Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/time";
        var httpResponse = await _httpClient.GetAsync(path);

        return await _responseHandler.HandleResponseAsync<BinanceServerTimeResponse>(httpResponse, cancellationToken);
    }

    public async Task<Result<object>> TestConnectivityAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/ping";

        var response = await _httpClient.GetAsync(path);

        return await _responseHandler.HandleResponseAsync<object>(response, cancellationToken);
    }

    public async Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/exchangeInfo";
        var httpResponse = await _httpClient.GetAsync(path);

        return await _responseHandler.HandleResponseAsync<BinanceExchangeInfoResponse>(httpResponse, cancellationToken);
    }

    public async Task<Result<BinanceAccountInformationResponse>> GetAccountInformationAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v3/account";

        var queryString = $"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var signature = CreateSignature(queryString);
        queryString += $"&signature={signature}";

        var response = await _httpClient.GetAsync($"{path}?{queryString}");

        return await _responseHandler.HandleResponseAsync<BinanceAccountInformationResponse>(response, cancellationToken);
    }

    public async Task<Result<IEnumerable<BinanceAccountBalanceResponse>>> GetAccountBalancesAsync(CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v3/balance";

        var queryString = $"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var signature = CreateSignature(queryString);
        queryString += $"&signature={signature}";

        var response = await _httpClient.GetAsync($"{path}?{queryString}");

        return await _responseHandler.HandleResponseAsync<IEnumerable<BinanceAccountBalanceResponse>>(response, cancellationToken);
    }

    public async Task<Result<NewOrderResponse>> NewOrderAsync(NewOrderRequest request, CancellationToken cancellationToken = default)
    {
        var path = "/fapi/v1/order";

        var queryString = $"timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&symbol={request.Symbol}";
        queryString += $"&side={request.Side}";
        queryString += $"&type={request.Type}";
        queryString += $"&quantity={request.Quantity}";

        var signature = CreateSignature(queryString);
        queryString += $"&signature={signature}";


        var response = await _httpClient.PostAsync($"{path}?{queryString}", null);

        return await _responseHandler.HandleResponseAsync<NewOrderResponse>(response, cancellationToken);
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
