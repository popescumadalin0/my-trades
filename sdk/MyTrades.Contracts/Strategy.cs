using System;

namespace MyTrades.Contracts;

public class Strategy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double WinRate { get; set; }
    public int TradesCount { get; set; }
    public decimal Profit { get; set; }
}

//todo: include these in the SDK
public class ApiResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public string ServerErrorMessage { get; set; }

    public ApiResponse()
    {
        Success = true;
        ErrorMessage = string.Empty;
        ServerErrorMessage = string.Empty;
    }

    public ApiResponse(string errorMessage, string serverErrorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
        ServerErrorMessage = serverErrorMessage;
    }

    public ApiResponse(Exception exception, string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
        ServerErrorMessage = exception.ToString();
    }

    public ApiResponse(Exception exception)
    {
        Success = false;
        ErrorMessage = "Something went wrong!";
        ServerErrorMessage = exception.ToString();
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }

    public ApiResponse(Exception exception, string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
        ServerErrorMessage = exception.ToString();
        Data = default;
    }

    public ApiResponse(Exception exception)
    {
        Success = false;
        ErrorMessage = "Something went wrong!";
        Data = default;
        ServerErrorMessage = exception.ToString();
    }

    public ApiResponse(T data)
    {
        Success = true;
        ErrorMessage = string.Empty;
        ServerErrorMessage = string.Empty;
        Data = data;
    }

    public ApiResponse(string errorMessage, string serverErrorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
        ServerErrorMessage = serverErrorMessage;
        Data = default;
    }
}