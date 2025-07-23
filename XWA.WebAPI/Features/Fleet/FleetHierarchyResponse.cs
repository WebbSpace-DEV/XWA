using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Squadron;

namespace XWA.WebAPI.Features.Fleet;

/// <summary>
/// The hierarchy-based fleet response model class.
/// </summary>
/// <remarks>
/// The constructor for the hierarchy-based fleet response model.
/// </remarks>
/// <param name="squadrons">The collection of hierarchy-base squadrons associated with the fleet.</param>
public class FleetHierarchyResponse(
    IList<SquadronHierarchyResponse> squadrons) : FleetBase()
{

    /// <summary>
    /// The collection of squadrons associate with a fleet.
    /// </summary>
    [Name("squadrons")]
    public IList<SquadronHierarchyResponse> Squadrons { get; set; } = squadrons;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
