using Microsoft.Extensions.DependencyInjection;
using MyTrades.Gateway.Refit.Responses;
using Refit;

namespace MyTrades.Gateway.Refit.Clients;

public interface IMockApi
{
    [Get("api/candle")]
    Task<CandleResponse> GetCandleAsync([Query]string symbol);
}

public interface IMockApiClient
{
    Task<ApiResponse<CandleResponse>> GetCandleAsync(string symbol);
}

public class MockApiClient(IMockApi api) : RefitApiClient<IMockApi>(api), IMockApiClient
{
    public Task<ApiResponse<CandleResponse>> GetCandleAsync(string symbol)
    {
        return ExecuteAsync(() => ApiClient.GetCandleAsync(symbol));
    }
}