namespace XWA.WebAPI.Features.PortalIcon;

/// <summary>
/// The portal icon service interface.
/// </summary>
public interface IPortalIconService
{
    /// <summary>
    /// The portal icon interface signature.
    /// </summary>
    /// <returns>The collection of portal icon response models.</returns>
    Task<IList<PortalIconResponse>> GetPortalIconsAsync();
}
