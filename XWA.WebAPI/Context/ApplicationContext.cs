using Microsoft.EntityFrameworkCore;
using XWA.WebAPI.Features.Book;

namespace XWA.WebAPI.Context;

/// <summary>
/// The application context class.
/// </summary>
/// <param name="options">DbContextOptions of type ApplicationContext.</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    // Default schema for the database context
    private const string DefaultSchema = "bookapi";

    /// <summary>
    /// DbSet to represent the collection of books in our database.
    /// </summary>
    public DbSet<BookModel> Books { get; set; }

    // Constructor to configure the database context

    /// <summary>
    /// OnModelCreating Event.
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder to be created.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
