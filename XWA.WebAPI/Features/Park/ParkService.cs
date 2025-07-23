using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Park;

/// <summary>
/// The park service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
/// <param name="fipsOptions">The FIPS codes from the appsettings.json file.</param>
public class ParkService(
    IOptions<CsvFileOptions> csvFileOptions,
    IOptions<FipsOptions> fipsOptions) : IParkService
{
    /// <summary>
    /// The park service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base park response models.</returns>
    public async Task<IList<ParkResponse>> GetParksAsync()
    {
        return await GetParks.Execute(csvFileOptions.Value.Parks, fipsOptions.Value.Oconus);
    }
}
