namespace XWA.WebAPI.Options;

/// <summary>
/// The class for accessing the bar-delimited randomizable range of collection sizes
/// from appsettings.json consumed using the Options Pattern.
/// </summary>
public class CollectionSizeOptions
{
    /// <summary>
    /// This is the literal for the section key in appsettings.json.
    /// </summary>
    public const string Key = "CollectionSize";

    /// <summary>
    /// The bar-delimited range of randomizable values for the collection of squadrons.
    /// </summary>
    public string Squadron { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable values for the collection of platforms.
    /// </summary>
    public string Platform { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable values for the collection of airframes.
    /// </summary>
    public string Airframe { get; set; } = string.Empty;

    /// <summary>
    /// The bar-delimited range of randomizable values for the collection of portal icons.
    /// </summary>
    public string PortalIcon { get; set; } = string.Empty;
}
