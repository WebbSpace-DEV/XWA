using Microsoft.EntityFrameworkCore;
using XWA.WebAPI.Features.Provision;

namespace XWA.WebAPI.Context;

/// <summary>
/// The application context class.
/// </summary>
/// <param name="options">DbContextOptions of type ApplicationContext.</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    // Default schema for the database context
    private const string DefaultSchema = "xwaapi";

    /// <summary>
    /// DbSet to represent the collection of provisions in our database.
    /// </summary>
    public DbSet<ProvisionBase> Provision { get; set; }

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
