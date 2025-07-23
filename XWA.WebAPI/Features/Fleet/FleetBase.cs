using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Fleet;

/// <summary>
/// The base fleet model class.
/// </summary>
public class FleetBase() : IScorable
{
    /// <summary>
    /// The score of the fleet, which is calculated from the average score of subordinate platforms.
    /// </summary>
    [Name("score")]
    public decimal Score { get; set; } = 0M;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"{GetLevel(Score),10}");
        sb.Append($"{Score,10:N0}");

        return sb.ToString();
    }
}
