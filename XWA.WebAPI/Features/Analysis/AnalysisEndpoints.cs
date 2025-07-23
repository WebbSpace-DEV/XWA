using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Analysis;

/// <summary>
/// The analysis endpoints class.
/// </summary>
internal static class AnalysisEndpoints
{
    private const string _TAG = "Analysis";

    /// <summary>
    /// Method to expose analysis endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get the Analysis
        builder.MapGet("/analysis/analysis", async (
            IAnalysisService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<CollectionSizeOptions> collectionSizeOptions,
            IOptions<ProvisionScoreOptions> provisionScoreOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            AnalysisResponse result = await service.GetAnalysisAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
