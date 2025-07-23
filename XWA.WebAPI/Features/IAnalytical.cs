using CsvHelper.Configuration.Attributes;

namespace XWA.WebAPI.Features;

/// <summary>
/// The interface for tracking the forward aggregation of score values.
/// </summary>
public interface IAnalytical
{
    /// <summary>
    /// Count of items at Level 1 (Green).
    /// </summary>
    [Name("level1Count")]
    public int Level1Count { get; set; }

    /// <summary>
    /// Count of items at Level 2 (Amber).
    /// </summary>
    [Name("level2Count")]
    public int Level2Count { get; set; }

    /// <summary>
    /// Count of items at Level 3 (Red).
    /// </summary>
    [Name("level3Count")]
    public int Level3Count { get; set; }

    /// <summary>
    /// Percentage of items at Level 1 (Green).
    /// </summary>
    [Name("level1Percent")]
    public decimal Level1Percent { get; set; }

    /// <summary>
    /// Percentage of items at Level 2 (Amber).
    /// </summary>
    [Name("level2Percent")]
    public decimal Level2Percent { get; set; }

    /// <summary>
    /// Percentage of items at Level 3 (Red).
    /// </summary>
    [Name("level3Percent")]
    public decimal Level3Percent { get; set; }
}
