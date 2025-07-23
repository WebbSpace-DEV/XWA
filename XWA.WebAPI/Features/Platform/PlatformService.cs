using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// The platform service class.
/// </summary>
/// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
public class PlatformService(
    IOptions<CollectionSizeOptions> collectionSizeOptions) : IPlatformService
{
    /// <summary>
    /// The hierarchy-based platform service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base platform response models.</returns>
    public async Task<IList<PlatformHierarchyResponse>> GetPlatformsAsync()
    {
        return await GetPlatforms.Execute(collectionSizeOptions);
    }
}
