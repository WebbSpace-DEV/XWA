namespace XWA.WebAPI.Features.Region;

/// <summary>
/// The region service interface.
/// </summary>
public interface IRegionService
{
    /// <summary>
    /// The region interface signature.
    /// </summary>
    /// <returns>The collection of region response models.</returns>
    Task<IList<RegionResponse>> GetRegionsAsync();
}
