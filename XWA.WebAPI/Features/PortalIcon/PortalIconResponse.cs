using CsvHelper.Configuration.Attributes;

namespace XWA.WebAPI.Features.PortalIcon;

/// <summary>
/// The portal icon response model class.
/// </summary>
/// <param name="shortTitle">The short title of the portal icon.</param>
/// <param name="longTitle">The long title of the portal icon.</param>
/// <param name="color">The background color of the portal icon.</param>
/// <param name="url">The href URL of the portal icon.</param>
public class PortalIconResponse(
        string shortTitle,
        string longTitle,
        string color,
        string url)
{
    /// <summary>
    /// The short title of the portal icon.
    /// </summary>
    [Name("shortTitle")]
    public string ShortTitle { get; set; } = shortTitle;

    /// <summary>
    /// The long title of the portal icon.
    /// </summary>
    [Name("longTitle")]
    public string LongTitle { get; set; } = longTitle;

    /// <summary>
    /// The background color of the portal icon.
    /// </summary>
    [Name("color")]
    public string Color { get; set; } = color;

    /// <summary>
    /// The href URL of the portal icon.
    /// </summary>
    [Name("url")]
    public string Url { get; set; } = url;
}
