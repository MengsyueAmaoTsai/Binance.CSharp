﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

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

    private async Task<Result<TResponse>> HandleResponseAsync<TResponse>(
        HttpResponseMessage httpResponse,
        CancellationToken cancellationToken = default)
    {
        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

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
        catch (JsonException)
        {
            _logger.LogError("Failed to deserialize response: {Content}", content);
            return Result<TResponse>.Failure(Error.Unexpected("0", "Failed to deserialize response"));
        }
    }
}
