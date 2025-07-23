using System.Text;
using XWA.Core.Converters;
using XWA.UI.Features.SessionFactory;
using XWA.UI.Features.User;

class Program
{
    public static int Main()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        WebApplication app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapGet("api/sessionFactory", async () => {
            string url = builder.Configuration["WebService:BaseAddress"]!;

            UserRequest user = new(
                Guid.NewGuid(),
                builder.Configuration["Credential:Email"]!,
                builder.Configuration["Credential:Password"]!
                );

            StringBuilder sb = new();

            using (HttpClient client = new())
            {
                StringContent content = new(JsonConverters<UserRequest>.Serialize(user), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.PostAsync($"{client.BaseAddress}/user/login", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    Task<string> task = response.Content.ReadAsStringAsync();
                    
                    sb.Append(task.Result);
                }
                else
                {
                    throw new Exception();
                }
            };

            return Results.Ok(new SessionFactoryResponse {
                BaseAddress = url,
                User = user,
                Version = System.Reflection.Assembly.GetAssembly(typeof(Program))!.GetName().Version!.ToString(),
                JwtToken = sb.ToString()
            });
        });

        app.Run();
        return 0;
    }
}
