using Microsoft.Extensions.Options;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.PortalIcon;

/// <summary>
/// The portal icon endpoints class.
/// </summary>
internal static class PortalIconEndpoints
{
    private const string _TAG = "PortalIcons";

    /// <summary>
    /// Method to expose portal icon endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapPortalIconEndpoints(this IEndpointRouteBuilder builder)
    {
        // Endpoint to get all portal icons.
        builder.MapGet("/portalIcon/portalIcons", async (
            IPortalIconService service,
            IOptions<CollectionSizeOptions> collectionSizeOptions) =>
        {
            IEnumerable<PortalIconResponse> result = await service.GetPortalIconsAsync();
            return Results.Ok(result);
        }).WithTags(_TAG);

        return builder;
    }
}
