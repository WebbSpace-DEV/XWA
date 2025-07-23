using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Region;

/// <summary>
/// The region endpoints class.
/// </summary>
internal static class RegionEndpoints
{
    private const string _TAG = "Regions";

    /// <summary>
    /// Method to expose region endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapRegionEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all regions
        builder.MapGet("/parkVisit/regions", async (
            IRegionService service,
            IOptions<CsvFileOptions> csvFileOptions) =>
        {
            IEnumerable<RegionResponse> result = await service.GetRegionsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG);

        return builder;
    }
}
