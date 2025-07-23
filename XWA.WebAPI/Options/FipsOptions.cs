namespace XWA.WebAPI.Options;

/// <summary>
/// The class for accessing the bar-delimited FIPS code values
/// from appsettings.json consumed using the Options Pattern.
/// </summary>
public class FipsOptions
{
    /// <summary>
    /// This is the literal for the section key in appsettings.json.
    /// </summary>
    public const string Key = "Fips";

    /// <summary>
    /// The bar-delimited collection of outside continental US ("OCONUS") FIPS options setting.
    /// </summary>
    public string Oconus { get; set; } = string.Empty;
}
