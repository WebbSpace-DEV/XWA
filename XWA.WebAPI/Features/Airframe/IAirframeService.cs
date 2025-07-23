namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The airframe service interface.
/// </summary>
public interface IAirframeService
{
    /// <summary>
    /// The hierarchy-based airframe interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base airframe response models.</returns>
    Task<IList<AirframeHierarchyResponse>> GetAirframesAsync();
}
