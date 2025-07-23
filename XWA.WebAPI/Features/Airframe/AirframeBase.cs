using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The base airframe model class.
/// </summary>
public class AirframeBase(
    string id,
    string platform) : IScorable
{
    /// <summary>
    /// The key id of the airframe.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The platform associated with the airframe.
    /// </summary>
    [Name("platform")]
    public string Platform { get; set; } = platform;

    /// <summary>
    /// The score of the airframe, which is calculated from the average score of subordinate provisions.
    /// </summary>
    [Name("score")]
    public decimal Score { get; set; } = 0M;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{Id,10}");
        sb.Append($"{Platform,10}");
        sb.Append($"{GetLevel(Score),10}");
        sb.Append($"{Score,10:N0}");

        return sb.ToString();
    }
}
