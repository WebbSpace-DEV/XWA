using Microsoft.Extensions.Options;
using System.Text;
using XWA.WebAPI.Options;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.PortalIcon;

/// <summary>
/// This procedure gets the collection of a specific user's portal icons.
/// </summary>
public class GetPortalIcons()
{
    /// <summary>
    /// The portal icon execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <returns>The collection of portal icons.</returns>
    public static async Task<IList<PortalIconResponse>> Execute(
        IOptions<CollectionSizeOptions> collectionSizeOptions)
    {
        IList<PortalIconResponse> results = [];

        await Task.Run(() =>
        {
            // Generate a collection of portal icons.
            for (int i = 1; i <= GetRandomBetween(collectionSizeOptions.Value.PortalIcon); i++)
            {
                results.Add(new PortalIconResponse(
                    $"Title {i:0000}",
                    $"Long Title {i:0000}",
                    $"#{GetRandomBetween(min: 0, max: 255):X2}{GetRandomBetween(min: 0, max: 255):X2}{GetRandomBetween(min: 0, max: 255):X2}",
                    $"Url {i:0000}"
                    ));
            }
        });

        return [.. results.OrderByDescending(GetSortCriteria)];
    }

    private static string GetSortCriteria(PortalIconResponse response)
    {
        StringBuilder sb = new();

        sb.Append($"{response.ShortTitle}.");
        sb.Append($"{response.LongTitle}.");
        sb.Append($"{response.Color}.");
        sb.Append($"{response.Url}.");

        return sb.ToString();
    }
}
