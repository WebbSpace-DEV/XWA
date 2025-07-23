namespace XWA.WebAPI.Features.Fleet;

/// <summary>
/// The fleet service interface.
/// </summary>
public interface IFleetService
{
    /// <summary>
    /// The hierarchy-based fleet interface signature.
    /// </summary>
    /// <returns>The hierarchy-base fleet response model.</returns>
    Task<FleetHierarchyResponse> GetFleetAsync();
}
