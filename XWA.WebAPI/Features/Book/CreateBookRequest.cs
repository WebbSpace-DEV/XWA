namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The create book request model for CQRS / Minimal API architecture.
/// </summary>
public class CreateBookRequest
{
    /// <summary>
    /// The title of the book.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// The author of the book.
    /// </summary>
    public string Author { get; init; } = string.Empty;

    /// <summary>
    /// The description of the book.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// The category of the book.
    /// </summary>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// The language of the book.
    /// </summary>
    public string Language { get; init; } = string.Empty;

    /// <summary>
    /// The total number of pages of the book.
    /// </summary>
    public int TotalPages { get; init; }
}
