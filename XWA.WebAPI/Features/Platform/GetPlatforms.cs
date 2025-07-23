using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// This procedure gets the platform information, typically consumed during squadron processing.
/// </summary>
public class GetPlatforms()
{
    /// <summary>
    /// The hierarchy-based platform execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <returns>The collection of platform response models.</returns>
    public static async Task<IList<PlatformHierarchyResponse>> Execute(
        IOptions<CollectionSizeOptions> collectionSizeOptions)
    {
        IList<PlatformHierarchyResponse> results = [];
        await Task.Run(() =>
        {
            // Generate a collection of platforms.
            for (int i = 1; i <= GetRandomBetween(collectionSizeOptions.Value.Platform); i++)
            {
                results.Add(new(
                        GetMockSignature(index: 21, length: 6, hasDash: true)));
            }
        });

        return [.. results.OrderBy(r => r.Id)];
    }
}
