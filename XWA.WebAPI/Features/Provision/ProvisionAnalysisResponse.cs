using CsvHelper.Configuration.Attributes;
using System.Text;
using XWA.Core.Constants;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The provision analysis response model class.
/// </summary>
public class ProvisionAnalysisResponse : ProvisionBase, IAnalytical
{
    /// <summary>
    /// The constructor for the provision analysis response model.
    /// </summary>
    /// <param name="type">The type of the provision.</param>
    /// <param name="name">The name of the provision.</param>
    public ProvisionAnalysisResponse(
        ProvisionTypes type,
        string name) : base(type, name)
    {
        Type = type;
        Name = name;
    }

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

        sb.Append($"{Level1Count,10:N0}");
        sb.Append($"{Level2Count,10:N0}");
        sb.Append($"{Level3Count,10:N0}");
        sb.Append($"{Level1Percent,10:N0}%");
        sb.Append($"{Level2Percent,10:N0}%");
        sb.Append($"{Level3Percent,10:N0}%");

        return sb.ToString();
    }
}
