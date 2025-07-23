using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Airframe;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Options;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// This procedure gets the squadron information, typically consumed during analysis processing.
/// </summary>
public class GetSquadrons()
{
    /// <summary>
    /// The hierarchy-based squadron execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="platformsPrototype">The collection of hierarchy-based prototype platform objects.</param>
    /// <param name="provisionsPrototype">The collection of hierarchy-based prototype provision objects.</param>
    /// <param name="airfieldsPrototype">The collection of hierarchy-based prototype airfield objects.</param>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
    /// <returns>The collection of squadron response models.</returns>
    public static async Task<IList<SquadronHierarchyResponse>> Execute(
        IList<PlatformHierarchyResponse> platformsPrototype,
        IList<ProvisionHierarchyResponse> provisionsPrototype,
        IList<AirfieldHierarchyResponse> airfieldsPrototype,
        IOptions<CollectionSizeOptions> collectionSizeOptions,
        IOptions<ProvisionScoreOptions> provisionScoreOptions)
    {
        IList<SquadronHierarchyResponse> results = [];

        // Use the IATA list for tracking airfields that are included in the airfields payload.
        IList<string> iataFactory = [];

        decimal overall = 0M;
        decimal counter = 0M;

        // Generate a collection of squadrons, each assigned a unique airfield.
        for (int i = 1; i <= GetRandomBetween(collectionSizeOptions.Value.Squadron); i++)
        {
            AirfieldHierarchyResponse airfield = airfieldsPrototype.ElementAt(GetRandomBetween(min: 0, max: airfieldsPrototype.Count - 1));

            // Add only unique airfield entries. This assures that the score of an airfield is
            // directly affiliated with thw score of the squadron. If a duplicate airfield is
            // already used, the number of added squadrons will be fewer than the number being
            // iterated, but that's acceptable for a prototype application and its "shim" data.
            if (!iataFactory.Contains(airfield.Id))
            {
                iataFactory.Add(airfield.Id);

                // Instantiate and scaffold the per-squadron airframe collection.
                IList<AirframeHierarchyResponse> airframes = await GetAirframes.Execute(platformsPrototype, provisionsPrototype, collectionSizeOptions, provisionScoreOptions);

                overall = 0M;
                counter = 0M;

                foreach (AirframeHierarchyResponse airframe in airframes)
                {
                    overall += airframe.Score;
                    counter++;
                }

                results.Add(new(
                                GetMockSignature(index: 0, length: 3, hasDash: false),
                                airframes,
                                airfield)
                {
                    Score = Math.Round(overall / counter, 0)
                });
            }
        }

        return [.. results.OrderBy(r => r.Id)];
    }
}
