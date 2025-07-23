namespace XWA.WebAPI.Options;

/// <summary>
/// The class for accessing the bar-delimited randomizable range of provision scores
/// from appsettings.json consumed using the Options Pattern.
/// </summary>
public class ProvisionScoreOptions
{
    /// <summary>
    /// This is the literal for the section key in appsettings.json.
    /// </summary>
    public const string Key = "ProvisionScore";

    /// <summary>
    /// The bar-delimited range of randomizable sensor scores.
    /// </summary>
    public string Sensor { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable servo scores.
    /// </summary>
    public string Servo { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable astromech droid scores.
    /// </summary>
    public string Droid { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable power scores.
    /// </summary>
    public string Power { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable shield scores.
    /// </summary>
    public string Shield { get; set; } = string.Empty;
}
