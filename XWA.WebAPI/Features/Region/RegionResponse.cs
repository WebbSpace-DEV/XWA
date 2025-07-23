using CsvHelper.Configuration.Attributes;

namespace XWA.WebAPI.Features.Region;

/// <summary>
/// The region response model class.
/// </summary>
/// <param name="id">The id of the region.</param>
/// <param name="name">The name of the region.</param>
public class RegionResponse(
    string id,
    string name)
{
    /// <summary>
    /// The key id of the region.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The name of the region.
    /// </summary>
    [Name("name")]
    public string Name { get; set; } = name;
}
