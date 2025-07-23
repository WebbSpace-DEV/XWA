namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book-feature exception message class invoked when no books can be found.
/// </summary>
public class NoBookFoundException() : Exception("No books found")
{
}
