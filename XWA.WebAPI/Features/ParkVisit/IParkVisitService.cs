namespace XWA.WebAPI.Features.ParkVisit;

/// <summary>
/// The park visit service interface.
/// </summary>
public interface IParkVisitService
{
    /// <summary>
    /// The park visit interface signature.
    /// </summary>
    /// <returns>The park visit response model.</returns>
    Task<ParkVisitResponse> GetParkVisitAsync();
}
