using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Park;
using XWA.WebAPI.Features.Region;

namespace XWA.WebAPI.Features.ParkVisit;

/// <summary>
/// The park visit response model class.
/// </summary>
/// <param name="regions">The collection of regions associated with the park visit.</param>
/// <param name="parks">The collection of parks associated with the park visit.</param>
public class ParkVisitResponse(
    IList<RegionResponse> regions,
    IList<ParkResponse> parks)
{
    /// <summary>
    /// The collection of regions associated with the park visit.
    /// </summary>
    [Name("regions")]
    public IList<RegionResponse> Regions { get; set; } = regions;

    /// <summary>
    /// The collection of parks associated with the park visit.
    /// </summary>
    [Name("parks")]
    public IList<ParkResponse> Parks { get; set; } = parks;
}
