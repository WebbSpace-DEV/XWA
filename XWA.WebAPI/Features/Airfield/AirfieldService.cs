using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Flight;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The airfield service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
public class AirfieldService(
    IOptions<CsvFileOptions> csvFileOptions) : IAirfieldService
{
    /// <summary>
    /// The hierarchy-based airfield service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base airfield response models.</returns>
    public async Task<IList<AirfieldHierarchyResponse>> GetAirfieldsAsync()
    {
        // Get the prototype collection of flights.
        IList<FlightHierarchyResponse> flightsPrototype = await GetFlights.Execute(csvFileOptions.Value.Flights);

        return await GetAirfields.Execute(csvFileOptions.Value.Airfields, flightsPrototype);
    }
}
