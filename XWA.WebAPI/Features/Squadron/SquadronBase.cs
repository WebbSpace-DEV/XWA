using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Squadron;

/// <summary>
/// The base squadron model class.
/// </summary>
public class SquadronBase(
    string id) : IScorable
{
    /// <summary>
    /// The key id of the squadron.
    /// </summary>
    [Name("id")]
    public string Id { get; set; } = id;

    /// <summary>
    /// The score of the squadron, which is calculated from the average score of subordinate airframes.
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
