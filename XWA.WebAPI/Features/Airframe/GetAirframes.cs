using Microsoft.Extensions.Options;
using XWA.Core.Constants;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Options;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// This procedure gets airframe information, typically consumed during platform processing.
/// </summary>
public class GetAirframes()
{
    /// <summary>
    /// The hierarchy-based airframe execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="platformsPrototype">The collection of hierarchy-based prototype platform objects.</param>
    /// <param name="provisionsPrototype">The collection of hierarchy-based prototype provision objects.</param>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
    /// <returns>The collection of airframe response models.</returns>
    public static async Task<IList<AirframeHierarchyResponse>> Execute(
        IList<PlatformHierarchyResponse> platformsPrototype,
        IList<ProvisionHierarchyResponse> provisionsPrototype,
        IOptions<CollectionSizeOptions> collectionSizeOptions,
        IOptions<ProvisionScoreOptions> provisionScoreOptions)
    {
        IList<AirframeHierarchyResponse> results = [];

        await Task.Run(() =>
        {
            bool isScored = false;

            decimal overall = 0M;
            decimal bias = 0M;

            // Generate a collection of airframes.
            for (int i = 1; i <= GetRandomBetween(collectionSizeOptions.Value.Airframe); i++)
            {
                // Instantiate and scaffold the per-airframe provision collection.
                IList<ProvisionHierarchyResponse> provisions = [];

                foreach (ProvisionHierarchyResponse provision in provisionsPrototype)
                {
                    provisions.Add(new(
                        provision.Type,
                        provision.Name)
                    {
                        Bias = provision.Bias,
                        Score = 0
                    });
                }

                overall = 0M;
                bias = 0M;

                foreach (ProvisionHierarchyResponse provision in provisions)
                {
                    isScored = true;
                    switch (provision.Type)
                    {
                        case ProvisionTypes.SENSOR:
                            provision.Score = GetRandomBetween(provisionScoreOptions.Value.Sensor);
                            break;
                        case ProvisionTypes.SERVO:
                            provision.Score = GetRandomBetween(provisionScoreOptions.Value.Servo);
                            break;
                        case ProvisionTypes.DROID:
                            provision.Score = GetRandomBetween(provisionScoreOptions.Value.Droid);
                            break;
                        case ProvisionTypes.POWER:
                            provision.Score = GetRandomBetween(provisionScoreOptions.Value.Power);
                            break;
                        case ProvisionTypes.SHIELD:
                            provision.Score = GetRandomBetween(provisionScoreOptions.Value.Shield);
                            break;
                        default:
                            isScored = false;
                            break;
                    }
                    if (isScored)
                    {
                        overall += provision.Score * provision.Bias;
                        bias += provision.Bias;
                    }
                }

                results.Add(new(
                    GetMockSignature(index: 20, length: 7, hasDash: false),
                    platformsPrototype[GetRandomBetween(min: 0, max: platformsPrototype.Count - 1)].Id,
                    string.Empty,
                    provisions)
                {
                    Score = Math.Round(overall / bias, 0)
                });
            }
        });

        return [.. results.OrderBy(r => r.Id)];
    }
}
