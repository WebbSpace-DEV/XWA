namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// The squadron service interface.
/// </summary>
public interface ISquadronService
{
    /// <summary>
    /// The hierarchy-based squadron interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base squadron response models.</returns>
    Task<IList<SquadronHierarchyResponse>> GetSquadronsAsync();
}
