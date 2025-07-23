using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using XWA.WebAPI.Features.Book;

namespace XWA.WebAPI.Exceptions;

/// <summary>
/// Global exception handler class implementing IExceptionHandler
/// </summary>
/// <param name="logger">The logger for the global exception handler.</param>
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Method to handle exceptions asynchronously.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="exception">The exception to be handled.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Boolean indicating whether the exception was handled.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Log the exception details
        logger.LogError(exception, "An error occurred while processing your request");

        ErrorResponse errorResponse = new()
        {
            Message = exception.Message,
            Title = exception.GetType().Name,
            // Determine the status code based on the type of exception
            StatusCode = exception switch
            {
                BadHttpRequestException => (int)HttpStatusCode.BadRequest,
                NoBookFoundException or BookDoesNotExistException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError,
            }
        };

        // Set the response status code
        httpContext.Response.StatusCode = errorResponse.StatusCode;

        // Write the error response as JSON
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        // Return true to indicate that the exception was handled
        return true;
    }
}
