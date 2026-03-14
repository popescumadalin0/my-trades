using System.Net;

namespace MyTrades.Gateway.Refit;

public class ApiResponse
{
    public bool Success { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }
}