using CsvHelper.Configuration.Attributes;
using System.Text;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The airfield analysis response model class.
/// </summary>
public class AirfieldAnalysisResponse : AirfieldBase, IAnalytical
{
    /// <summary>
    /// The constructor for the airfield analysis response model.
    /// </summary>
    /// <param name="id">The key id ("IATA") for the airfield.</param>
    /// <param name="name">The airfield name.</param>
    /// <param name="city">The city where the airfield is located.</param>
    /// <param name="state">The state where the airfield is located.</param>
    /// <param name="country">The country where the airfield is located.</param>
    /// <param name="latitude">The latitude where the airfield is geo-located.</param>
    /// <param name="longitude">The longitude where the airfield is geo-located.</param>
    public AirfieldAnalysisResponse(
        string id,
        string name,
        string city,
        string state,
        string country,
        decimal latitude,
        decimal longitude) : base(id, name, city, state, country, latitude, longitude)
    {
        Id = id;
        Name = name;
        City = city;
        State = state;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// The squadron associated with the airfield.
    /// </summary>
    [Name("squadron")]
    public string Squadron { get; set; } = string.Empty;

    /// <summary>
    /// Count of items at Level 1 (Green).
    /// </summary>
    [Name("level1Count")]
    public int Level1Count { get; set; } = 0;

    /// <summary>
    /// Count of items at Level 2 (Amber).
    /// </summary>
    [Name("level2Count")]
    public int Level2Count { get; set; } = 0;

    /// <summary>
    /// Count of items at Level 3 (Red).
    /// </summary>
    [Name("level3Count")]
    public int Level3Count { get; set; } = 0;

    /// <summary>
    /// Percentage of items at Level 1 (Green).
    /// </summary>
    [Name("level1Percent")]
    public decimal Level1Percent { get; set; } = 0M;

    /// <summary>
    /// Percentage of items at Level 2 (Amber).
    /// </summary>
    [Name("level2Percent")]
    public decimal Level2Percent { get; set; } = 0M;

    /// <summary>
    /// Percentage of items at Level 3 (Red).
    /// </summary>
    [Name("level3Percent")]
    public decimal Level3Percent { get; set; } = 0M;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new(base.ToString());

        sb.Append($"{Squadron,10}");
        sb.Append($"{Level1Count,10:N0}");
        sb.Append($"{Level2Count,10:N0}");
        sb.Append($"{Level3Count,10:N0}");
        sb.Append($"{Level1Percent,10:N0}%");
        sb.Append($"{Level2Percent,10:N0}%");
        sb.Append($"{Level3Percent,10:N0}%");

        return sb.ToString();
    }
}
