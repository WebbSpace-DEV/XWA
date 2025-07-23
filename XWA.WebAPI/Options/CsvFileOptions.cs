namespace XWA.WebAPI.Options;

/// <summary>
/// The class for accessing the CSV file path and name values
/// from appsettings.json consumed using the Options Pattern.
/// </summary>
public class CsvFileOptions
{
    /// <summary>
    /// This is the literal for the section key in appsettings.json.
    /// </summary>
    public const string Key = "CsvFile";

    /// <summary>
    /// The provisions CSV file path and name options setting.
    /// </summary>
    public string Provisions { get; set; } = string.Empty;

    /// <summary>
    /// The airfields CSV file path and name options setting.
    /// </summary>
    public string Airfields { get; set; } = string.Empty;

    /// <summary>
    /// The flights CSV file path and name options setting.
    /// </summary>
    public string Flights { get; set; } = string.Empty;

    /// <summary>
    /// The regions CSV file path and name options setting.
    /// </summary>
    public string Regions { get; set; } = string.Empty;

    /// <summary>
    /// The parks CSV file path and name options setting.
    /// </summary>
    public string Parks { get; set; } = string.Empty;
}
