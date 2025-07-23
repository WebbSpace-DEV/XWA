namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The provision service interface.
/// </summary>
public interface IProvisionService
{
    /// <summary>
    /// The hierarchy-based provision interface signature.
    /// </summary>
    /// <returns>The collection of hierarchy-base provision response models.</returns>
    Task<IList<ProvisionHierarchyResponse>> GetProvisionsAsync();
}
