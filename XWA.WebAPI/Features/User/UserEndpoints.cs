namespace XWA.WebAPI.Features.User;

/// <summary>
/// The user endpoints class.
/// </summary>
internal static class UserEndpoints
{
    private const string _TAG = "User";

    /// <summary>
    /// Method to expose user endpoints.
    /// </summary>
    /// <param name="builder">The route-builder object.</param>
    /// <returns>The builder with endpoint-mapped routes.</returns>
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("user/login", async (LoginUser.Request request, LoginUser useCase) =>
            await useCase.Handle(request))
            .WithTags(_TAG);

        return builder;
    }
}
