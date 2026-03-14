using MyTrades.Gateway.Exceptions;
using MyTrades.Gateway.Refit.Clients;
using MyTrades.Gateway.Refit.Responses;

namespace MyTrades.Gateway;

public interface ICandleGatewayService
{
    Task<CandleResponse> GetCandlesAsync(string symbol);
}

public class CandleGatewayService : ICandleGatewayService
{
    private readonly IMockApiClient _mockApiClient;

    public CandleGatewayService(IMockApiClient mockApiClient)
    {
        _mockApiClient = mockApiClient;
    }

    public async Task<CandleResponse> GetCandlesAsync(string symbol)
    {
        var response = await _mockApiClient.GetCandleAsync(symbol);

        if (response.Success)
        {
            return response.Data;
        }
        
        throw new ApiException(response.StatusCode, response.ErrorMessage);
    }
}