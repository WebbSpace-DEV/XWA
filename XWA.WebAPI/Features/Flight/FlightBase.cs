using CsvHelper.Configuration.Attributes;
using System.Text;

namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// The base flight model class.
/// </summary>
public class FlightBase(
    string orig,
    string dest,
    int count)
{
    /// <summary>
    /// The origination IATA for the flight.
    /// </summary>
    [Name("orig")]
    public string Orig { get; set; } = orig;

    /// <summary>
    /// The destination IATA for the flight.
    /// </summary>
    [Name("dest")]
    public string Dest { get; set; } = dest;

    /// <summary>
    /// The count of flights between the origination IATA and the destination IATA.
    /// </summary>
    [Name("count")]
    public int Count { get; set; } = count;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{Orig,10}");
        sb.Append($"{Dest,10}");
        sb.Append($"{Count,10:N0}");

        return sb.ToString();
    }
}
