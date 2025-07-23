using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Provision;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The hierarchy-based airframe response model class.
/// </summary>
public class AirframeHierarchyResponse : AirframeBase
{
    /// <summary>
    /// The constructor for the hierarchy-based airframe response model.
    /// </summary>
    /// <param name="id">The id of the airframe.</param>
    /// <param name="platform">The platform on which the airframe is based.</param>
    /// <param name="squadron">The squadron to which the airframe is assigned.</param>
    /// <param name="provisions">The collection of provisions for the airframe.</param>
    public AirframeHierarchyResponse(
        string id,
        string platform,
        string squadron,
        IList<ProvisionHierarchyResponse> provisions) : base(id, platform)
    {
        Id = id;
        Platform = platform;
        Squadron = squadron;
        Provisions = provisions;
    }

    /// <summary>
    /// The squadron to which the airframe is assigned.
    /// </summary>
    [Name("squadron")]
    public string Squadron { get; set; }

    /// <summary>
    /// The collection of provisions associated with an airframe.
    /// </summary>
    [Name("provisions")]
    public IList<ProvisionHierarchyResponse> Provisions { get; set; }

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
