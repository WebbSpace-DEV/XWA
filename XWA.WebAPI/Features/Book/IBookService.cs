namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book service interface.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Add a new book.
    /// </summary>
    /// <param name="createBookRequest">The request model containing the payload to be created ("stored").</param>
    /// <returns>Details of the created book</returns>
    Task<BookResponse> AddBookAsync(CreateBookRequest createBookRequest);

    /// <summary>
    /// Get a book by its ID.
    /// </summary>
    /// <param name="id">ID of the book</param>
    /// <returns>>Details of the book</returns>
    Task<BookResponse> GetBookByIdAsync(Guid id);

    /// <summary>
    /// Get all books.
    /// </summary>
    /// <returns>Collection of all books.</returns>
    Task<IList<BookResponse>> GetBooksAsync();

    /// <summary>
    /// Update a book.
    /// </summary>
    /// <param name="id">The id of the book to be updated.</param>
    /// <param name="updateBookRequest">The request model containing the updated payload.</param>
    /// <returns>The response model containing the updated book payload.</returns>
    Task<BookResponse> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest);

    /// <summary>
    /// Delete a book by its ID.
    /// </summary>
    /// <param name="id">ID of the book to be deleted</param>
    /// <returns>True if the book was deleted, false otherwise.</returns>
    Task<bool> DeleteBookAsync(Guid id);
}
