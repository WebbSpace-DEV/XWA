using XWA.Core.Constants;
using XWA.WebAPI.Extensions;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Airframe;
using XWA.WebAPI.Features.Analysis;
using XWA.WebAPI.Features.Book;
using XWA.WebAPI.Features.Fleet;
using XWA.WebAPI.Features.Flight;
using XWA.WebAPI.Features.Park;
using XWA.WebAPI.Features.ParkVisit;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.PortalIcon;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Features.Region;
using XWA.WebAPI.Features.Squadron;
using XWA.WebAPI.Features.User;

namespace XWA.WebAPI;

class Program
{
    public static int Main()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder.AddApplicationServices();

        WebApplication app = builder.Build();

        app.UseCors(Global.WhiteList);

        app.UseRouting();
        // UseAuthentication() and UseAuthorization() must come *after* UseRouting().
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            // Add in a development-only endpoint to view "behind the scenes" information.
            app.MapGet("/config", async request =>
            {
                string config = ((IConfigurationRoot)app.Configuration).GetDebugView();
                await request.Response.WriteAsync(config);
            });
        }

        app.UseExceptionHandler();

        app.MapGroup("/api/v1/")
            .WithTags("Book Endpoints")
            .MapBookEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Analysis Endpoints")
            .MapAnalysisEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Fleet Endpoints")
            .MapFleetEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Platform Endpoints")
            .MapPlatformEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Squadron Endpoints")
            .MapSquadronEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Airframe Endpoints")
            .MapAirframeEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Provision Endpoints")
            .MapProvisionEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Airfield Endpoints")
            .MapAirfieldEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Flight Endpoints")
            .MapFlightEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Park Visit Endpoints")
            .MapParkVisitEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Region Endpoints")
            .MapRegionEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Park Endpoints")
            .MapParkEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("Portal Icon Endpoints")
            .MapPortalIconEndpoints();

        app.MapGroup("/xwa_ws/api/v1/")
            .WithTags("User Endpoints")
            .MapUserEndpoints();

        app.Run();
        return 0;
    }
}
