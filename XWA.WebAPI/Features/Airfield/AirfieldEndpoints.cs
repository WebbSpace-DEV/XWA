using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The airfield endpoints class.
/// </summary>
internal static class AirfieldEndpoints
{
    private const string _TAG = "Airfields";

    /// <summary>
    /// Method to expose airfield endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapAirfieldEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all airfields.
        builder.MapGet("/analysis/airfields", async (
            IAirfieldService service,
            IOptions<CsvFileOptions> csvFileOptions) =>
        {
            IEnumerable<AirfieldHierarchyResponse> result = await service.GetAirfieldsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG);

        return builder;
    }
}
