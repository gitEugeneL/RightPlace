using System.Security.Claims;
using System.Text;
using Api.Helpers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Api;

public static class ConfigureServices
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        
        /*** Auth config ***/
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration.GetSection("Authentication:Key").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1) // allowed time deviation, 5min - default
                };
            });

        /*** Auth policy ***/
        services.AddAuthorizationBuilder()
            .AddPolicy(AppConstants.BaseAuthPolicy, policy =>
                policy
                    .RequireClaim(ClaimTypes.Email)
                    .RequireClaim(ClaimTypes.NameIdentifier));
        
        /*** Swagger Config ***/
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = """ Standard JWT Bearer Authorization with refresh token. Example: "bearer" {token} """,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        
        /*** API versioning ***/
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
        
        services.AddCors();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        return services;
    }
}