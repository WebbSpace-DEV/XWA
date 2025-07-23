using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Flight;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// The squadron service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
/// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
/// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
/// <param name="provisionBiasOptions">The pilot-survivability provision bias values from the appsettings.json file.</param>
public class SquadronService(
    IOptions<CsvFileOptions> csvFileOptions,
    IOptions<CollectionSizeOptions> collectionSizeOptions,
    IOptions<ProvisionScoreOptions> provisionScoreOptions,
    IOptions<ProvisionBiasOptions> provisionBiasOptions) : ISquadronService
{
    /// <summary>
    /// The hierarchy-based squadron service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base squadron response models.</returns>
    public async Task<IList<SquadronHierarchyResponse>> GetSquadronsAsync()
    {
        // Get the prototype collection of platforms.
        IList<PlatformHierarchyResponse> platformsPrototype = await GetPlatforms.Execute(collectionSizeOptions);

        // Get the prototype collection of flights.
        IList<FlightHierarchyResponse> flightsPrototype = await GetFlights.Execute(csvFileOptions.Value.Flights);

        // Get the prototype collection of airfields.
        IList<AirfieldHierarchyResponse> airfieldsPrototype = await GetAirfields.Execute(csvFileOptions.Value.Airfields, flightsPrototype);

        // Get the prototype collection of provisions.
        IList<ProvisionHierarchyResponse> provisionsPrototype = await GetProvisions.Execute(csvFileOptions.Value.Provisions, provisionBiasOptions);

        return await GetSquadrons.Execute(platformsPrototype, provisionsPrototype, airfieldsPrototype, collectionSizeOptions, provisionScoreOptions);
    }
}
