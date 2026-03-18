using System;
using System.Net;
using System.Threading.Tasks;

namespace MyTrades.Gateway.Refit;

public abstract class RefitApiClient<TApi>(TApi api)
    where TApi : class
{
    protected readonly TApi ApiClient = api;

    protected static async Task<ApiResponse<T>> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            var data = await action();

            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }
        catch (global::Refit.ApiException ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = ex.StatusCode,
                ErrorMessage = ex.Content
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>()
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message
            };
        }
    }

    protected static async Task<ApiResponse> ExecuteAsync(Func<Task> action)
    {
        try
        {
            await action();

            return new ApiResponse
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
            };
        }
        catch (global::Refit.ApiException ex)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = ex.StatusCode,
                ErrorMessage = ex.Content
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse()
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message
            };
        }
    }
}