using System.Text;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.Binance.Authentication;
using RichillCapital.Binance.Extensions;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.UsdM;

internal sealed class BinanceUsdMRestClient(
    ILogger<BinanceUsdMRestClient> _logger,
    HttpClient _httpClient,
    BinanceSignatureService _signatureService) :
    IBinanceUsdMRestClient
{
    private const string ApiKey = "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92";
    private const string SecretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

    private const int RecvWindow = 60000;

    public async Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default)
    {
        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}";
        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(SecretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", ApiKey);

        var path = "fapi/v1/order";
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             path,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response);
    }

    private async Task<Result<TBinanceResponse>> HandleResponseAsync<TBinanceResponse>(HttpResponseMessage response)
    {
        var uri = response.RequestMessage?.RequestUri;
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Request {path} failed. Status: {status}. Content: {content}",
                uri,
                response.StatusCode,
                responseContent);

            var error = await response.ToErrorAsync();
            _logger.LogError("Transformed {error}", error);

            return Result<TBinanceResponse>.Failure(error);
        }

        var binanceResponse = JsonConvert.DeserializeObject<TBinanceResponse>(responseContent);
        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}, Content: {binanceResponse}");

        return Result<TBinanceResponse>.With(binanceResponse!);
    }

    private async Task<Result> HandleResponseAsync(HttpResponseMessage response)
    {
        var uri = response.RequestMessage?.RequestUri;
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Request {path} failed. Status: {status}. Content: {content}",
                uri,
                response.StatusCode,
                responseContent);

            var error = await response.ToErrorAsync();
            _logger.LogError("Transformed {error}", error);

            return Result.Failure(error);
        }

        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}, {responseContent}");

        return Result.Success;
    }
}