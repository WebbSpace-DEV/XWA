namespace XWA.WebAPI.Features.User;

/// <summary>
/// The user response model class.
/// </summary>
/// <param name="id">The id of the user.</param>
/// <param name="email">The email address of the user.</param>
public class UserResponse(
    Guid id,
    string email
    )
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public Guid Id { get; set; } = id;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string Email { get; set; } = email;
}
