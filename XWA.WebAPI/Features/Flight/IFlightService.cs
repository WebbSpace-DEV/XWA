namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// The flight service interface.
/// </summary>
public interface IFlightService
{
    /// <summary>
    /// The hierarchy-based flight interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base flight response models.</returns>
    Task<IList<FlightHierarchyResponse>> GetFlightsAsync();
}
