using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The airframe endpoints class.
/// </summary>
internal static class AirframeEndpoints
{
    private const string _TAG = "Airframes";

    /// <summary>
    /// Method to expose airframe endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapAirframeEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all airframes.
        builder.MapGet("/analysis/airframes", async (
            IAirframeService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<CollectionSizeOptions> collectionSizeOptions,
            IOptions<ProvisionScoreOptions> provisionScoreOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            IList<AirframeHierarchyResponse> result = await service.GetAirframesAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }

}
