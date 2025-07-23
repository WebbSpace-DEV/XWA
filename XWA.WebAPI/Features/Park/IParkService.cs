namespace XWA.WebAPI.Features.Park;

/// <summary>
/// The park service interface.
/// </summary>
public interface IParkService
{
    /// <summary>
    /// The park interface signature.
    /// </summary>
    /// <returns>The collection of park response models.</returns>
    Task<IList<ParkResponse>> GetParksAsync();
}
