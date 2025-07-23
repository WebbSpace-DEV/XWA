using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Region;

/// <summary>
/// The region service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
public class RegionService(
    IOptions<CsvFileOptions> csvFileOptions) : IRegionService
{
    /// <summary>
    /// The region service wrapper method.
    /// </summary>
    /// <returns>The collection of region response models.</returns>
    public async Task<IList<RegionResponse>> GetRegionsAsync()
    {
        return await GetRegions.Execute(csvFileOptions.Value.Regions);
    }
}
