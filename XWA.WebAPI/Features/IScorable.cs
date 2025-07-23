namespace XWA.WebAPI.Features;

/// <summary>
/// The interface for persisting the overall score.
/// </summary>
public interface IScorable
{
    /// <summary>
    /// The score of the entity consuming this interface.
    /// </summary>
    public decimal Score { get; set; }
}
