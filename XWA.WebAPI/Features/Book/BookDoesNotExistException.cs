namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book-feature exception message class invoked when a specific book id cannot be found.
/// </summary>
/// <param name="id">The id of the book that was not found.</param>
public class BookDoesNotExistException(int id) : Exception($"Book with id {id} does not exist")
{
}
