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
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtManager, JwtManager>();
        
        // Db connection config --------------------------------------------------------------------
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            // option.UseNpgsql(configuration.GetConnectionString("PgSQLConnection"));
            option.UseSqlite(configuration.GetConnectionString("SQLiteConnection"));
        });
        
        // Db initializer config -------------------------------------------------------------------
        ApplicationDbContextInitializer
            .Init(services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}