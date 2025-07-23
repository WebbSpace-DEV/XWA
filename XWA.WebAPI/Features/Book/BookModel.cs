namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book model.
/// </summary>
public class BookModel
{
    /// <summary>
    /// The id of the book.
    /// </summary>
    public Guid Id { get; set; }

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
