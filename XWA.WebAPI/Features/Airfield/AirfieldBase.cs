using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The base airfield model class.
/// </summary>
public class AirfieldBase(
    string id,
    string name,
    string city,
    string state,
    string country,
    decimal latitude,
    decimal longitude) : IScorable
{
    /// <summary>
    /// The key id ("IATA") of the airfield.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The name of the airfield.
    /// </summary>
    [Name("name")]
    public string Name { get; set; } = name;

    /// <summary>
    /// The city where the airfield is located.
    /// </summary>
    [Name("city")]
    public string City { get; set; } = city;

    /// <summary>
    /// The state where the airfield is located.
    /// </summary>
    [Name("state")]
    public string State { get; set; } = state;

    /// <summary>
    /// The country where the airfield is located.
    /// </summary>
    [Name("country")]
    public string Country { get; set; } = country;

    /// <summary>
    /// The latitude where the airfield is geo-located.
    /// </summary>
    [Name("latitude")]
    public decimal Latitude { get; set; } = latitude;

    /// <summary>
    /// The longitude where the airfield is geo-located.
    /// </summary>
    [Name("longitude")]
    public decimal Longitude { get; set; } = longitude;

    /// <summary>
    /// The score of the airfield, which is calculated directly from its associated squadron.
    /// </summary>
    [Optional]
    [Name("score")]
    public decimal Score { get; set; } = 0M;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{Id,10}");
        sb.Append($"{GetLevel(Score),10}");
        sb.Append($"{Score,10:N0}");
        sb.Append($"{Name,45}");
        sb.Append($"{City,45}");
        sb.Append($"{State,5}");
        sb.Append($"{Country,5}");
        sb.Append($"{Latitude,15:N8}");
        sb.Append($"{Longitude,15:N8}");

        return sb.ToString();
    }
}
