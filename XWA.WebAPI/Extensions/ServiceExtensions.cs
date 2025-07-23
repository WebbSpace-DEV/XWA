using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using XWA.Core.Constants;
using XWA.WebAPI.Context;
using XWA.WebAPI.Exceptions;
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
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Extensions;

/// <summary>
/// The service extensions class: a catalog of capabilities ("extensions") used in dependency injection.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Method for adding dependency injection services to the Application Builder.
    /// </summary>
    /// <param name="builder">The host application builder object.</param>
    /// <exception cref="ArgumentNullException">The exception thrown is the method is improperly instantiated.</exception>
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(builder.Configuration);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Global.WhiteList,
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
        });

        // Adding the database context
        builder.Services.AddDbContext<ApplicationContext>(configure =>
        {
            configure.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
        });

        builder.Services.AddSingleton<TokenProvider>();

        /*
         *
         * This code is based on:
         *
         * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
         *
         * and
         *
         * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0
         *
         */
        builder.Services.Configure<CsvFileOptions>(
            builder.Configuration.GetSection(CsvFileOptions.Key));
        builder.Services.Configure<FipsOptions>(
            builder.Configuration.GetSection(FipsOptions.Key));
        builder.Services.Configure<CollectionSizeOptions>(
            builder.Configuration.GetSection(CollectionSizeOptions.Key));
        builder.Services.Configure<ProvisionScoreOptions>(
            builder.Configuration.GetSection(ProvisionScoreOptions.Key));
        builder.Services.Configure<ProvisionBiasOptions>(
            builder.Configuration.GetSection(ProvisionBiasOptions.Key));

        // Adding validators from the current assembly
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // The JWT login functionality is based on https://www.youtube.com/watch?v=6DWJIyipxzw
        builder.Services.AddScoped<LoginUser>();

        builder.Services.AddScoped<IAnalysisService, AnalysisService>();
        builder.Services.AddScoped<IFleetService, FleetService>();
        builder.Services.AddScoped<IPlatformService, PlatformService>();
        builder.Services.AddScoped<ISquadronService, SquadronService>();
        builder.Services.AddScoped<IAirframeService, AirframeService>();
        builder.Services.AddScoped<IProvisionService, ProvisionService>();
        builder.Services.AddScoped<IAirfieldService, AirfieldService>();
        builder.Services.AddScoped<IFlightService, FlightService>();

        builder.Services.AddScoped<IParkVisitService, ParkVisitService>();
        builder.Services.AddScoped<IRegionService, RegionService>();
        builder.Services.AddScoped<IParkService, ParkService>();

        builder.Services.AddScoped<IPortalIconService, PortalIconService>();

        builder.Services.AddScoped<IBookService, BookService>();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddProblemDetails();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        _ = builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            OpenApiSecurityScheme securityScheme = new()
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            OpenApiSecurityRequirement securityRequirement = new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            c.AddSecurityRequirement(securityRequirement);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "XWA WebAPI",
                Version = "v1",
                Description = "X-Wing Advisor (XWA) prototype using Minimal API in .NET"
            });

            // Set the comments path for the Swagger JSON and UI.
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}
