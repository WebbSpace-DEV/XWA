using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Platform;

/// <summary>
/// The base platform model class.
/// </summary>
public class PlatformBase(
    string id) :IScorable
{
    /// <summary>
    /// The key id of the platform.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The score of the platform, which is calculated from the average score of subordinate airframes.
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
        sb.Append($"{GetLevel(Score),10}");
        sb.Append($"{Score,10:N0}");

        return sb.ToString();
    }
}
