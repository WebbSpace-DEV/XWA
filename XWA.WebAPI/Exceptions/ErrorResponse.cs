namespace XWA.WebAPI.Exceptions;

/// <summary>
/// The error response model class.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// The error title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The error status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
