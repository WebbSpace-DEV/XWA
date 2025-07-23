using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Fleet;

/// <summary>
/// The fleet endpoints class.
/// </summary>
internal static class FleetEndpoints
{
    private const string _TAG = "Fleet";

    /// <summary>
    /// Method to expose fleet endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapFleetEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get the fleet
        builder.MapGet("/analysis/fleet", async (
            IFleetService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<CollectionSizeOptions> collectionSizeOptions,
            IOptions<ProvisionScoreOptions> provisionScoreOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            FleetHierarchyResponse result = await service.GetFleetAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
