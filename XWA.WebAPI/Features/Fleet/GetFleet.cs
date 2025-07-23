using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Features.Squadron;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Fleet;

/// <summary>
/// This procedure gets fleet information, typically consumed during fleet processing.
/// </summary>
public class GetFleet()
{
    /// <summary>
    /// The hierarchy-based fleet execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="platformsPrototype">The collection of hierarchy-based prototype platform objects.</param>
    /// <param name="provisionsPrototype">The collection of hierarchy-based prototype provision objects.</param>
    /// <param name="airfieldsPrototype">The collection of hierarchy-based prototype airfield objects.</param>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
    /// <returns>The fleet response model.</returns>
    public static async Task<FleetHierarchyResponse> Execute(
        IList<PlatformHierarchyResponse> platformsPrototype,
        IList<ProvisionHierarchyResponse> provisionsPrototype,
        IList<AirfieldHierarchyResponse> airfieldsPrototype,
        IOptions<CollectionSizeOptions> collectionSizeOptions,
        IOptions<ProvisionScoreOptions> provisionScoreOptions)
    {
        // Instantiate and scaffold the fleet squadron collection.
        IList<SquadronHierarchyResponse> squadrons = await GetSquadrons.Execute(platformsPrototype, provisionsPrototype, airfieldsPrototype, collectionSizeOptions, provisionScoreOptions);

        decimal overall = 0M;
        decimal counter = 0M;

        foreach (SquadronHierarchyResponse squadron in squadrons)
        {
            overall += squadron.Score;
            counter++;
        }

        return new(squadrons)
        {
            Score = Math.Round(overall / counter, 0)
        };
    }
}
