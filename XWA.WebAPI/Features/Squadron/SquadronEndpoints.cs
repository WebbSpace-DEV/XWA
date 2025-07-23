using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// The squadron endpoints class.
/// </summary>
internal static class SquadronEndpoints
{
    private const string _TAG = "Squadrons";

    /// <summary>
    /// Method to expose squadron endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapSquadronEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all squadrons
        builder.MapGet("/analysis/squadrons", async (
            ISquadronService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<CollectionSizeOptions> collectionSizeOptions,
            IOptions<ProvisionScoreOptions> provisionScoreOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            IList<SquadronHierarchyResponse> result = await service.GetSquadronsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
