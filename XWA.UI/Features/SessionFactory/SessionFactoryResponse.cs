using XWA.UI.Features.User;

namespace XWA.UI.Features.SessionFactory;

public class SessionFactoryResponse
{
    public string BaseAddress { get; set; } = string.Empty;

    public UserRequest? User {  get; set; }

    public string Version { get; set; } = string.Empty;

    public string JwtToken { get; set; } = string.Empty;
}
