using System;
using System.Net;

namespace MyTrades.Gateway.Exceptions;

public class ApiException : Exception
{
    /// <summary>
    /// HTTP response status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// HTTP Response content as string.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class.
    /// </summary>
    public ApiException(
        HttpStatusCode statusCode,
        string? errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}