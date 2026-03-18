using System.Threading;
using System.Threading.Tasks;
using MyTrades.Gateway.Refit.Responses;
using Refit;

namespace MyTrades.Gateway.Refit.Clients;

public interface IMockApi
{
    [Get("api/candle")]
    Task<CandleResponse> GetCandleAsync([Query]string symbol, CancellationToken cancellationToken);
}

public interface IMockApiClient
{
    Task<ApiResponse<CandleResponse>> GetCandleAsync(string symbol, CancellationToken cancellationToken = default);
}

public class MockApiClient(IMockApi api) : RefitApiClient<IMockApi>(api), IMockApiClient
{
    public Task<ApiResponse<CandleResponse>> GetCandleAsync(string symbol, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(() => ApiClient.GetCandleAsync(symbol, cancellationToken));
    }
}