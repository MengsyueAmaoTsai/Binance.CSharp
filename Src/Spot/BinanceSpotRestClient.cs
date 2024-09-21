using System.Text;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using RichillCapital.Binance.Spot.Contracts;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Binance.Spot;

internal sealed class BinanceSpotRestClient(
    ILogger<BinanceSpotRestClient> _logger,
    HttpClient _httpClient,
    BinanceSpotSignatureService _signatureService) :
    IBinanceSpotRestClient
{
    private const int RecvWindow = 60000;

    public async Task<Result> PingAsync(CancellationToken cancellationToken = default)
    {
        var path = BinanceSpotApiRoutes.General.Ping;
        var response = await _httpClient.GetAsync(path);

        return await HandleResponseAsync(response);
    }

    public async Task<Result<BinanceServerTimeResponse>> GetServerTimeAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BinanceSpotApiRoutes.General.ServerTime);
        return await HandleResponseAsync<BinanceServerTimeResponse>(response);
    }

    public async Task<Result<BinanceExchangeInfoResponse>> GetExchangeInfoAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(BinanceSpotApiRoutes.General.ExchangeInfo);
        return await HandleResponseAsync<BinanceExchangeInfoResponse>(response);
    }

    public async Task<Result> NewOrderAsync(
        string symbol,
        string side,
        string type,
        decimal quantity,
        CancellationToken cancellationToken = default)
    {
        var apiKey = "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92";
        var secretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}";
        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(secretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);

        var path = BinanceSpotApiRoutes.Trading.NewOrder;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             BinanceSpotApiRoutes.Trading.NewOrder,
             new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded"),
             cancellationToken);

        return await HandleResponseAsync(response);
    }

    public async Task<Result> TestNewOrderAsync(string symbol, string side, string type, decimal quantity, CancellationToken cancellationToken = default)
    {
        var apiKey = "guVqJIzZ29JZx2BTv9VbxxOr7IehQIIRRXABm53rawtThH0XcD8EeyzUtMbIaQ92";
        var secretKey = "BPwSSG45zE8ABiZ6Zm4t9gJFJMo19ExjBqOQlmLcOM5LgfyYP6V5biYrsUkZfXxm";

        var queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}";
        queryString += $"&recvWindow={RecvWindow}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(secretKey, queryString)}";

        _logger.LogInformation("Final query string: {queryString}", queryString);

        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);

        var path = BinanceSpotApiRoutes.Trading.NewOrder;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             BinanceSpotApiRoutes.Trading.TestNewOrder,
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

            var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(responseContent);
            var error = CreateError(response.GetErrorType(), errorResponse!);

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

            var errorResponse = JsonConvert.DeserializeObject<BinanceErrorResponse>(responseContent);
            var error = CreateError(response.GetErrorType(), errorResponse!);

            _logger.LogError("Transformed {error}", error);

            return Result.Failure(error);
        }

        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}, {responseContent}");

        return Result.Success;
    }

    private static Error CreateError(
        ErrorType errorType,
        BinanceErrorResponse errorResponse)
    {
        var code = GetErrorCode(errorResponse);
        return errorType switch
        {
            ErrorType.Validation => Error.Invalid(code, errorResponse!.Message),
            ErrorType.Unauthorized => Error.Unauthorized(code, errorResponse!.Message),
            ErrorType.Forbidden => Error.Forbidden(code, errorResponse!.Message),
            ErrorType.NotFound => Error.NotFound(code, errorResponse!.Message),
            ErrorType.Conflict => Error.Conflict(code, errorResponse!.Message),
            ErrorType.Unexpected => Error.Unexpected(code, errorResponse!.Message),
            _ => Error.Unexpected(code, errorResponse!.Message)
        };
    }

    private static string GetErrorCode(BinanceErrorResponse errorResponse)
    {
        var suffix = errorResponse switch
        {
            { Code: -2010 } => "NewOrderRejected",
            _ => throw new NotImplementedException($"Error code for {errorResponse} is not defined."),
        };

        return $"BinanceSpot.{suffix}";
    }
}
