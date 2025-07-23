using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// The platform endpoints class.
/// </summary>
internal static class PlatformEndpoints
{
    private const string _TAG = "Platforms";

    /// <summary>
    /// Method to expose platform endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapPlatformEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all platforms.
        builder.MapGet("/analysis/platforms", async (
            IPlatformService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<CollectionSizeOptions> collectionSizeOptions,
            IOptions<ProvisionScoreOptions> provisionScoreOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            IList<PlatformHierarchyResponse> result = await service.GetPlatformsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
