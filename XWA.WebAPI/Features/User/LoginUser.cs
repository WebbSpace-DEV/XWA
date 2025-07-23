using System.Text;

namespace XWA.WebAPI.Features.User;

internal sealed class LoginUser(TokenProvider tokenProvider, IConfiguration configuration)
{
    public sealed record Request(
        Guid Id,
        string Email,
        string Password);

    public async Task<string> Handle(Request request)
    {
        StringBuilder token = new();
        await Task.Run(() => {
            bool isMatch = true;
            isMatch = isMatch && string.Equals(request.Email, configuration["Credential:Email"]!, StringComparison.OrdinalIgnoreCase);
            isMatch = isMatch && string.Equals(request.Password, configuration["Credential:Password"]!, StringComparison.Ordinal);
            if (isMatch)
            {
                UserResponse user = new(
                    request.Id,
                    request.Email);
                token.Append(tokenProvider.Create(user));
            }
        });

        return token.ToString();
    }
}
