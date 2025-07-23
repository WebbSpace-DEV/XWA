namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// The hierarchy-based platform response model class.
/// </summary>
public class PlatformHierarchyResponse : PlatformBase
{
    /// <summary>
    /// The constructor for the hierarchy-based platform response model.
    /// </summary>
    /// <param name="id">The id of the platform.</param>
    public PlatformHierarchyResponse(
        string id) : base(id)
    {
        Id = id;
    }

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
