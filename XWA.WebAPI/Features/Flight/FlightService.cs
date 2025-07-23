using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// The flight service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values.</param>
public class FlightService(
    IOptions<CsvFileOptions> csvFileOptions) : IFlightService
{
    /// <summary>
    /// The hierarchy-based flight service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base flight response models.</returns>
    public async Task<IList<FlightHierarchyResponse>> GetFlightsAsync()
    {
        return await GetFlights.Execute(csvFileOptions.Value.Flights);
    }
}
