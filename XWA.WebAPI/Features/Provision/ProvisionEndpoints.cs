using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The provision endpoints class.
/// </summary>
internal static class ProvisionEndpoints
{
    private const string _TAG = "Provisions";

    /// <summary>
    /// Method to expose provision endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapProvisionEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all provisions.
        builder.MapGet("/analysis/provisions", async (
            IProvisionService service,
            IOptions<CsvFileOptions> csvFileOptions,
            IOptions<ProvisionBiasOptions> provisionBiasOptions) =>
        {
            IEnumerable<ProvisionHierarchyResponse> result = await service.GetProvisionsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
