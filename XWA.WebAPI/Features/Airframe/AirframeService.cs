using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The airframe service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
/// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
/// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
/// <param name="provisionBiasOptions">The pilot-survivability provision bias values from the appsettings.json file.</param>
public class AirframeService(
    IOptions<CsvFileOptions> csvFileOptions,
    IOptions<CollectionSizeOptions> collectionSizeOptions,
    IOptions<ProvisionScoreOptions> provisionScoreOptions,
    IOptions<ProvisionBiasOptions> provisionBiasOptions) : IAirframeService
{
    /// <summary>
    /// The hierarchy-based airframe service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base airframe response models.</returns>
    public async Task<IList<AirframeHierarchyResponse>> GetAirframesAsync()
    {
        // Get the prototype collection of platforms.
        IList<PlatformHierarchyResponse> platformsPrototype = await GetPlatforms.Execute(collectionSizeOptions);

        // Get the prototype collection of provisions.
        IList<ProvisionHierarchyResponse> provisionsPrototype = await GetProvisions.Execute(csvFileOptions.Value.Provisions, provisionBiasOptions);

        return await GetAirframes.Execute(platformsPrototype, provisionsPrototype, collectionSizeOptions, provisionScoreOptions);
    }
}
