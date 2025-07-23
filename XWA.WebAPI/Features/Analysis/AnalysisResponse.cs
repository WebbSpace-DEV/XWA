using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Airframe;
using XWA.WebAPI.Features.Fleet;
using XWA.WebAPI.Features.Flight;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Features.Squadron;

namespace XWA.WebAPI.Features.Analysis;

/// <summary>
/// The analysis response model class.
/// </summary>
/// <param name="fleetPrototype">The hierarchy-based prototype fleet object.</param>
/// <param name="fleet">The analysis-based fleet response model.</param>
/// <param name="platforms">The collection of analysis-base platform response models.</param>
/// <param name="squadrons">The collection of analysis-base squadron response models.</param>
/// <param name="airframes">The collection of analysis-base airframe response models.</param>
/// <param name="provisions">The collection of analysis-base provision response models.</param>
/// <param name="airfields">The collection of analysis-base airfield response models.</param>
/// <param name="flights">The collection of analysis-base flight response models.</param>
public class AnalysisResponse(
    FleetHierarchyResponse fleetPrototype,
    FleetAnalysisResponse fleet,
    IList<PlatformAnalysisResponse> platforms,
    IList<SquadronAnalysisResponse> squadrons,
    IList<AirframeAnalysisResponse> airframes,
    IList<ProvisionAnalysisResponse> provisions,
    IList<AirfieldAnalysisResponse> airfields,
    IList<FlightAnalysisResponse> flights)
{
    /// <summary>
    /// The prototype hierarchy-based fleet response model.
    /// </summary>
    [Name("fleetPrototype")]
    public FleetHierarchyResponse FleetPrototype { get; set; } = fleetPrototype;

    /// <summary>
    /// The analysis-based fleet response model.
    /// </summary>
    [Name("fleet")]
    public FleetAnalysisResponse Fleet { get; set; } = fleet;

    /// <summary>
    /// The collection of analysis-base platform response models.
    /// </summary>
    [Name("platforms")]
    public IList<PlatformAnalysisResponse> Platforms { get; set; } = platforms;

    /// <summary>
    /// The collection of analysis-base squadron response models.
    /// </summary>
    [Name("squadrons")]
    public IList<SquadronAnalysisResponse> Squadrons { get; set; } = squadrons;

    /// <summary>
    /// The collection of analysis-base airframe response models.
    /// </summary>
    [Name("airframes")]
    public IList<AirframeAnalysisResponse> Airframes { get; set; } = airframes;

    /// <summary>
    /// The collection of analysis-base provision response models.
    /// </summary>
    [Name("provisions")]
    public IList<ProvisionAnalysisResponse> Provisions { get; set; } = provisions;

    /// <summary>
    /// The collection of analysis-base airfield response models.
    /// </summary>
    [Name("airfields")]
    public IList<AirfieldAnalysisResponse> Airfields { get; set; } = airfields;

    /// <summary>
    /// The collection of analysis-base flight response models.
    /// </summary>
    [Name("flights")]
    public IList<FlightAnalysisResponse> Flights { get; set; } = flights;
}
