using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Client.Services;

public class StrategyHttpService : IStrategyService
{
    private readonly HttpClient _http;

    public StrategyHttpService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<List<Strategy>>> GetStrategiesAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<Strategy>>>("api/strategies");
            return response ?? new ApiResponse<List<Strategy>>("Empty response", "No response body.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Strategy>>(ex);
        }
    }

    public async Task<ApiResponse> AddOrUpdateStrategyAsync(Strategy strategy)
    {
        try
        {
            var httpResponse = await _http.PostAsJsonAsync("api/strategies", strategy);
            if (!httpResponse.IsSuccessStatusCode)
            {
                return new ApiResponse("Request failed", $"Status code: {httpResponse.StatusCode}");
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse>();
            return response ?? new ApiResponse("Empty response", "No response body.");
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex);
        }
    }
}
