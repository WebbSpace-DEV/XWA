using Microsoft.Extensions.Options;
using XWA.WebAPI.Features.Park;
using XWA.WebAPI.Features.Region;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.ParkVisit;

/// <summary>
/// The park visit service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values.</param>
/// <param name="fipsOptions">The FIPS codes from the appsettings.json file.</param>
public class ParkVisitService(
    IOptions<CsvFileOptions> csvFileOptions,
    IOptions<FipsOptions> fipsOptions) : IParkVisitService
{
    /// <summary>
    /// The park visit service wrapper method.
    /// </summary>
    /// <returns>The park visit response model.</returns>
    public async Task<ParkVisitResponse> GetParkVisitAsync()
    {
        IList<RegionResponse> regionsPrototype = await GetRegions.Execute(csvFileOptions.Value.Regions);

        IList<ParkResponse> parksPrototype = await GetParks.Execute(csvFileOptions.Value.Parks, fipsOptions.Value.Oconus);

        return await GetParkVisits.Execute(regionsPrototype, parksPrototype);
    }
}
