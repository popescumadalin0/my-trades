using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyTrades.Contracts.Interfaces;
using MyTrades.Contracts.Models;

namespace MyTrades.Client.Services;

public class TradeHttpService : ITradeService
{
    private readonly HttpClient _http;

    public TradeHttpService(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResponse<List<Trade>>> GetTradesAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<Trade>>>("api/trades");
            return response ?? new ApiResponse<List<Trade>>("Empty response", "No response body.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Trade>>(ex);
        }
    }

    public async Task<ApiResponse> AddOrUpdateTradeAsync(Trade trade)
    {
        try
        {
            var httpResponse = await _http.PostAsJsonAsync("api/trades", trade);
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
