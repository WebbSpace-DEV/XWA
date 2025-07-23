namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// The platform service interface.
/// </summary>
public interface IPlatformService
{
    /// <summary>
    /// The hierarchy-based platform interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base platform response models.</returns>
    Task<IList<PlatformHierarchyResponse>> GetPlatformsAsync();
}
