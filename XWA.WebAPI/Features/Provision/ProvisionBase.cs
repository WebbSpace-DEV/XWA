using CsvHelper.Configuration.Attributes;
using System.Text;
using XWA.Core.Constants;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The base provision model class.
/// </summary>
public class ProvisionBase(
    ProvisionTypes type,
    string name) : IScorable
{
    /// <summary>
    /// The type of the provision.
    /// </summary>
    [Name("type")]
    public ProvisionTypes Type { get; set; } = type;

    /// <summary>
    /// The name of the provision.
    /// </summary>
    [Name("name")]
    public string Name { get; set; } = name;

    /// <summary>
    /// The score of the provision.
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

        sb.Append($"{Type,10}");
        sb.Append($"{Name,20}");
        sb.Append($"{GetLevel(Score),10}");
        sb.Append($"{Score,10:N0}");

        return sb.ToString();
    }
}
