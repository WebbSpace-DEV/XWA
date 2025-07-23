using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Airframe;

namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// The hierarchy-based squadron response model class.
/// </summary>
public class SquadronHierarchyResponse : SquadronBase
{
    /// <summary>
    /// The constructor for the hierarchy-based squadron response model.
    /// </summary>
    /// <param name="id">The id of the squadron.</param>
    /// <param name="airframes">The collection of airframes assigned to the squadron.</param>
    /// <param name="airfield">The airfield associated with the squadron.</param>
    public SquadronHierarchyResponse(
        string id,
        IList<AirframeHierarchyResponse> airframes,
        AirfieldHierarchyResponse airfield) : base(id)
    {
        Id = id;
        Airframes = airframes;
        Airfield = airfield;
    }

    /// <summary>
    /// The collection of airframes associate with a squadron.
    /// </summary>
    [Name("airframes")]
    public IList<AirframeHierarchyResponse> Airframes { get; set; }

    /// <summary>
    /// The airfield associate with a squadron.
    /// </summary>
    [Name("airfield")]
    public AirfieldHierarchyResponse Airfield { get; set; }

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
