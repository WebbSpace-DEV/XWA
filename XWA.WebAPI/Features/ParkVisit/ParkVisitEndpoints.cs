using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.ParkVisit;

/// <summary>
/// The park visit endpoints class.
/// </summary>
internal static class ParkVisitEndpoints
{
    private const string _TAG = "ParkVisits";

    /// <summary>
    /// Method to expose park visit endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapParkVisitEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get the ParkVisit
        builder.MapGet("/parkVisit/parkVisits", async (
            IParkVisitService service,
            IOptions<CsvFileOptions> csvFileOptions) =>
        {
            ParkVisitResponse result = await service.GetParkVisitAsync();
            return Results.Ok(result);
        }).WithTags(_TAG);

        return builder;
    }
}
