using Microsoft.EntityFrameworkCore;
using XWA.WebAPI.Context;

namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book service class.
/// </summary>
/// <param name="context">The dependency-injected application context.</param>
/// <param name="logger">The dependency-injected logger.</param>
public class BookService(
    ApplicationContext context,
    ILogger<BookService> logger) : IBookService
{
    /// <summary>
    /// Add a new book.
    /// </summary>
    /// <param name="createBookRequest">The request model containing the payload to be created ("stored").</param>
    /// <returns>Details of the created book</returns>
    public async Task<BookResponse> AddBookAsync(CreateBookRequest createBookRequest)
    {
        try
        {
            BookModel book = new()
            {
                Title = createBookRequest.Title,
                Author = createBookRequest.Author,
                Description = createBookRequest.Description,
                Category = createBookRequest.Category,
                Language = createBookRequest.Language,
                TotalPages = createBookRequest.TotalPages
            };

            // Add the updateBookRequest to the database
            context.Books.Add(book);
            await context.SaveChangesAsync();
            logger.LogInformation("Book added successfully.");

            // Return the details of the created updateBookRequest
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                Language = book.Language,
                TotalPages = book.TotalPages
            };
        }
        catch (Exception ex)
        {
            logger.LogError("Error adding updateBookRequest: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Get a book by its ID.
    /// </summary>
    /// <param name="id">ID of the book</param>
    /// <returns>>Details of the book</returns>
    public async Task<BookResponse> GetBookByIdAsync(Guid id)
    {
        try
        {
            // Find the updateBookRequest by its ID
            BookModel? book = await context.Books.FindAsync(id);
            if (book == null)
            {
                logger.LogWarning("Book with ID {Id} not found.", id);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            // Return the details of the updateBookRequest
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                Language = book.Language,
                TotalPages = book.TotalPages
            };
        }
        catch (Exception ex)
        {
            logger.LogError("Error retrieving updateBookRequest: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Get all books.
    /// </summary>
    /// <returns>Collection of all books.</returns>
    public async Task<IList<BookResponse>> GetBooksAsync()
    {
        try
        {
            // Get all books from the database
            List<BookModel> books = await context.Books.ToListAsync();

            // Return the details of all books
            return [.. books.Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                Language = book.Language,
                TotalPages = book.TotalPages
            })];
        }
        catch (Exception ex)
        {
            logger.LogError("Error retrieving books: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Update a book.
    /// </summary>
    /// <param name="id">The id of the book to be updated.</param>
    /// <param name="updateBookRequest">The request model containing the updated payload.</param>
    /// <returns>The response model containing the updated book payload.</returns>
    public async Task<BookResponse> UpdateBookAsync(Guid id, UpdateBookRequest updateBookRequest)
    {
        try
        {
            // Find the existing updateBookRequest by its ID
            BookModel? existingBook = await context.Books.FindAsync(id);
            if (existingBook == null)
            {
                logger.LogWarning("Book with ID {Id} not found.", id);
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            // Update the updateBookRequest details
            existingBook.Title = updateBookRequest.Title;
            existingBook.Author = updateBookRequest.Author;
            existingBook.Description = updateBookRequest.Description;
            existingBook.Category = updateBookRequest.Category;
            existingBook.Language = updateBookRequest.Language;
            existingBook.TotalPages = updateBookRequest.TotalPages;

            // Save the changes to the database
            await context.SaveChangesAsync();
            logger.LogInformation("Book updated successfully.");

            // Return the details of the updated updateBookRequest
            return new BookResponse
            {
                Id = existingBook.Id,
                Title = existingBook.Title,
                Author = existingBook.Author,
                Description = existingBook.Description,
                Category = existingBook.Category,
                Language = existingBook.Language,
                TotalPages = existingBook.TotalPages
            };
        }
        catch (Exception ex)
        {
            logger.LogError("Error updating updateBookRequest: {Message}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Delete a book by its ID.
    /// </summary>
    /// <param name="id">ID of the book to be deleted</param>
    /// <returns>True if the book was deleted, false otherwise.</returns>
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        try
        {
            // Find the book by its ID
            BookModel? book = await context.Books.FindAsync(id);
            if (book == null)
            {
                logger.LogWarning("Book with ID {Id} not found.", id);
                return false;
            }

            // Remove the book from the database
            context.Books.Remove(book);
            await context.SaveChangesAsync();
            logger.LogInformation("Book with ID {Id} deleted successfully.", id);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError("Error deleting book: {Message}", ex.Message);
            throw;
        }
    }
}
