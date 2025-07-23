namespace XWA.WebAPI.Features.Book;

/// <summary>
/// The book endpoints class.
/// </summary>
internal static class BookEndpoints
{
    private const string _TAG = "Books";

    /// <summary>
    /// Method to expose book endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to add a new book
        builder.MapPost("/books", async (CreateBookRequest createBookRequest, IBookService bookService) =>
        {
            BookResponse result = await bookService.AddBookAsync(createBookRequest);
            return Results.Created($"/books/{result.Id}", result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        // Endpoint to get all books.
        builder.MapGet("/books", async (IBookService bookService) =>
        {
            IEnumerable<BookResponse> result = await bookService.GetBooksAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        // Endpoint to get a book by ID
        builder.MapGet("/books/{id:guid}", async (Guid id, IBookService bookService) =>
        {
            BookResponse result = await bookService.GetBookByIdAsync(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }).WithTags(_TAG)
        .RequireAuthorization();

        // Endpoint to update a book by ID
        builder.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest updateBookRequest, IBookService bookService) =>
        {
            BookResponse result = await bookService.UpdateBookAsync(id, updateBookRequest);
            return result != null ? Results.Ok(result) : Results.NotFound();
        }).WithTags(_TAG)
        .RequireAuthorization();

        // Endpoint to delete a book by ID
        builder.MapDelete("/books/{id:guid}", async (Guid id, IBookService bookService) =>
        {
            bool result = await bookService.DeleteBookAsync(id);
            return result ? Results.NoContent() : Results.NotFound();
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
