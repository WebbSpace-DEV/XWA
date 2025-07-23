using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Park;

/// <summary>
/// The park endpoints class.
/// </summary>
internal static class ParkEndpoints
{
    private const string _TAG = "Parks";

    /// <summary>
    /// Method to expose park endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapParkEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all parks.
        builder.MapGet("/parkVisit/parks", async (
            IParkService service,
            IOptions<CsvFileOptions> csvFileOptions) =>
        {
            IEnumerable<ParkResponse> result = await service.GetParksAsync();
            return Results.Ok(result);
        }).WithTags(_TAG);

        return builder;
    }
}
