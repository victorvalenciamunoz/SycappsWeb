using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SycappsWeb.Server.Data;
using System.Text;

namespace SycappsWeb.Server.StartupConfig;

public static class ServicesConfig
{
    public static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "JWT Authorization header info using bearer tokens",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[] { }
            }
        };

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var title = "Sycapps API";
            var description = "Web API developed by Sycapps to provide info to different apps.";
            var terms = new Uri("http://sycapps.net");
            var license = new OpenApiLicense()
            {
                Name = "Sycapps licence info"
            };
            var contact = new OpenApiContact()
            {
                Name = "Sycapps helpdesk",
                Email = "info@sycapps.net",
                Url = new Uri("http://sycapps.net")
            };

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = title,
                Description = description,
                TermsOfService = terms,
                License = license,
                Contact = contact
            });
            options.AddSecurityDefinition("bearerAuth", securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });
    }
    public static void AddApiVersioningServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
    }
    public static void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
        };
        opts.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = async ctx =>
            {
                var putBreakpointHere = true;
                var exceptionMessage = ctx.Exception;
            },
        };
    });
        builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }
}
