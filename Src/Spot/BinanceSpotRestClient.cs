using System.Net;
using System.Net.Http.Json;
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
        CancellationToken cancellationToken = default)
    {
        var apiKey = "";
        var secretKey = "";
        var queryString = $"symbol={symbol}&side={side}&type={type}&recvWindow=60000&timestamp={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        queryString += $"&signature={_signatureService.Sign(queryString, secretKey)}";
        _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", apiKey);

        var path = BinanceSpotApiRoutes.Trading.NewOrder;
        _logger.LogInformation("Invoke path: {path}", path);

        var response = await _httpClient.PostAsync(
             "api/v3/order",
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

            var error = response switch
            {
                { StatusCode: HttpStatusCode.BadRequest } => Error.Invalid(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Unauthorized } => Error.Unauthorized(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Forbidden } => Error.Forbidden(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.NotFound } => Error.NotFound(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Conflict } => Error.Conflict(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.InternalServerError } => Error.Unexpected(errorResponse!.Code.ToString(), errorResponse!.Message),
                _ => Error.Unexpected(errorResponse!.Code.ToString(), errorResponse!.Message)
            };

            _logger.LogInformation("Transformed error response: {errorResponse}", errorResponse);

            return Result<TBinanceResponse>.Failure(error);
        }

        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}");
        var binanceResponse = await response.Content.ReadFromJsonAsync<TBinanceResponse>();

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

            var error = response switch
            {
                { StatusCode: HttpStatusCode.BadRequest } => Error.Invalid(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Unauthorized } => Error.Unauthorized(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Forbidden } => Error.Forbidden(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.NotFound } => Error.NotFound(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.Conflict } => Error.Conflict(errorResponse!.Code.ToString(), errorResponse!.Message),
                { StatusCode: HttpStatusCode.InternalServerError } => Error.Unexpected(errorResponse!.Code.ToString(), errorResponse!.Message),
                _ => Error.Unexpected(errorResponse!.Code.ToString(), errorResponse!.Message)
            };

            _logger.LogInformation("Transformed error response: {errorResponse}", errorResponse);

            return Result.Failure(error);
        }

        _logger.LogInformation($"Success send request: {uri}. Status: {response.StatusCode}");

        return Result.Success;
    }
}
