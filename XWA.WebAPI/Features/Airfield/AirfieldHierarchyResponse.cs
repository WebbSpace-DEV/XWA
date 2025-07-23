using CsvHelper.Configuration.Attributes;
using XWA.WebAPI.Features.Flight;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// The hierarchy-based airfield response model class.
/// </summary>
public class AirfieldHierarchyResponse : AirfieldBase
{
    /// <summary>
    /// The constructor for the hierarchy-based airfield response model.
    /// </summary>
    /// <param name="id">The key id ("IATA") for the airfield.</param>
    /// <param name="name">The airfield name.</param>
    /// <param name="city">The city where the airfield is located.</param>
    /// <param name="state">The state where the airfield is located.</param>
    /// <param name="country">The country where the airfield is located.</param>
    /// <param name="latitude">The latitude where the airfield is geo-located.</param>
    /// <param name="longitude">The longitude where the airfield is geo-located.</param>
    /// <param name="flights">The collection of flights subordinate to the airfield</param>
    public AirfieldHierarchyResponse(
        string id,
        string name,
        string city,
        string state,
        string country,
        decimal latitude,
        decimal longitude,
        IList<FlightHierarchyResponse> flights) : base(id, name, city, state, country, latitude, longitude)
    {
        Id = id;
        Name = name;
        City = city;
        State = state;
        Country = country;
        Latitude = latitude;
        Longitude = longitude;
        Flights = flights;
    }

    /// <summary>
    /// The collection of flights associated associate with an airfield.
    /// </summary>
    [Optional]
    [Name("flights")]
    public IList<FlightHierarchyResponse> Flights { get; set; } = [];

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
