using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// The flight endpoints class.
/// </summary>
internal static class FlightEndpoints
{
    private const string _TAG = "Flights";

    /// <summary>
    /// Method to expose flight endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapFlightEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all flights.
        builder.MapGet("/analysis/flights", async (
            IFlightService service,
            IOptions<CsvFileOptions> csvFileOptions) =>
        {
            IList<FlightHierarchyResponse> result = await service.GetFlightsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG)
        .RequireAuthorization();

        return builder;
    }
}
