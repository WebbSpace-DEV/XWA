using CsvHelper.Configuration.Attributes;

namespace XWA.WebAPI.Features.Park;

/// <summary>
/// The park response model class.
/// </summary>
/// <param name="id">The key id of the park.</param>
/// <param name="name">The name of the park.</param>
/// <param name="type">The type of the park.</param>
/// <param name="region">The region that contains the park.</param>
/// <param name="fips">The FIPS code that contains the park.</param>
/// <param name="visitors">The average annual number of the visitors to the park.</param>
/// <param name="acreage">The total acreage of the park.</param>
/// <param name="latitude">The geo-location latitude of the park.</param>
/// <param name="longitude">The geo-location longitude of the park.</param>
public class ParkResponse(
        string id,
        string name,
        string type,
        string region,
        string fips,
        int visitors,
        int acreage,
        decimal latitude,
        decimal longitude)
{
    /// <summary>
    /// The key id of the park.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The name of the park.
    /// </summary>
    [Name("name")]
    public string Name { get; set; } = name;

    /// <summary>
    /// The type of the park.
    /// </summary>
    [Name("type")]
    public string Type { get; set; } = type;

    /// <summary>
    /// The region that contains the park.
    /// </summary>
    [Name("region")]
    public string Region { get; set; } = region;

    /// <summary>
    /// The FIPS code that contains the park.
    /// </summary>
    [Name("fips")]
    public string Fips { get; set; } = fips;

    /// <summary>
    /// The average annual number of the visitors to the park.
    /// </summary>
    [Name("visitors")]
    public int Visitors { get; set; } = visitors;

    /// <summary>
    /// The total acreage of the park.
    /// </summary>
    [Name("acreage")]
    public int Acreage { get; set; } = acreage;

    /// <summary>
    /// The geo-location latitude of the park.
    /// </summary>
    [Name("latitude")]
    public decimal Latitude { get; set; } = latitude;

    /// <summary>
    /// The geo-location longitude of the park.
    /// </summary>
    [Name("longitude")]
    public decimal Longitude { get; set; } = longitude;
}
