using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.PortalIcon;

/// <summary>
/// The portal icon service class.
/// </summary>
/// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
public class PortalIconService(
    IOptions<CollectionSizeOptions> collectionSizeOptions) : IPortalIconService
{
    /// <summary>
    /// The portal icon service wrapper method.
    /// </summary>
    /// <returns>The collection of portal icon response models.</returns>
    public async Task<IList<PortalIconResponse>> GetPortalIconsAsync()
    {
        return await GetPortalIcons.Execute(collectionSizeOptions);
    }
}
