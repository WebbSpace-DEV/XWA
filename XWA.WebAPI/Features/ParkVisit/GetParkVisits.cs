using XWA.WebAPI.Features.Park;
using XWA.WebAPI.Features.Region;

namespace XWA.WebAPI.Features.ParkVisit;

/// <summary>
/// This procedure gets the complex collection of information for park visits.
/// </summary>
public class GetParkVisits()
{
    /// <summary>
    /// The park visit execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="regions">The collection of regions associated with the park visit.</param>
    /// <param name="parks">The collection of parks associated with the park visit.</param>
    /// <returns>The park visit response model.</returns>
    public static async Task<ParkVisitResponse> Execute(
        IList<RegionResponse> regions,
        IList<ParkResponse> parks)
    {
        ParkVisitResponse parkVisit = new([], []);
        await Task.Run(() =>
        {
            parkVisit = new ParkVisitResponse([.. regions.OrderBy(r => r.Name)], [.. parks.OrderBy(r => r.Name)]);
        });
        return parkVisit;
    }
}
