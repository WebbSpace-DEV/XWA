namespace XWA.UI.Features.User;

public class UserRequest(
    Guid id,
    string email,
    string password)
{
    public Guid Id { get; set; } = id;

    public string Email { get; set; } = email;

    public string Password { get; set; } = password;
}
