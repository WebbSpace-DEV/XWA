namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The airfield service interface.
/// </summary>
public interface IAirfieldService
{
    /// <summary>
    /// The hierarchy-based airfield interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base airfield response models.</returns>
    Task<IList<AirfieldHierarchyResponse>> GetAirfieldsAsync();
}
