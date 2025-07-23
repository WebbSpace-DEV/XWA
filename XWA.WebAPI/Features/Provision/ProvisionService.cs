using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The provision service class.
/// </summary>
/// <param name="csvFileOptions">The CSV file path and name values from the appsettings.json file.</param>
/// <param name="provisionBiasOptions">The pilot-survivability provision bias values from the appsettings.json file.</param>
public class ProvisionService(
    IOptions<CsvFileOptions> csvFileOptions,
    IOptions<ProvisionBiasOptions> provisionBiasOptions) : IProvisionService
{
    /// <summary>
    /// The hierarchy-based provision service wrapper method.
    /// </summary>
    /// <returns>The collection of hierarchy-base provision response models.</returns>
    public async Task<IList<ProvisionHierarchyResponse>> GetProvisionsAsync()
    {
        return await GetProvisions.Execute(csvFileOptions.Value.Provisions, provisionBiasOptions);
    }
}
