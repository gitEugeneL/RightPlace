using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtManager, JwtManager>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        // Add DB connection -----------------------------------------------------------------------
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("PgSQLConnection"));
            // option.UseSqlite(configuration.GetConnectionString("SQLiteConnection"));
        });
        
        ApplicationDbContextInitializer
            .Init(services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}