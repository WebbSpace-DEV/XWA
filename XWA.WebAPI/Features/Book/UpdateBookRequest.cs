namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The update book request model for CQRS / Minimal API architecture.
/// </summary>
public class UpdateBookRequest
{
    /// <summary>
    /// The title of the book.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The author of the book.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// The description of the book.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The category of the book.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The language of the book.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// The total number of pages of the book.
    /// </summary>
    public int TotalPages { get; set; }
}
