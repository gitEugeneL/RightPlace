using Application.Common.Interfaces;
using Infrastructure.FileService;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IAdvertRepository, AdvertRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IInformationRepository, InformationRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtManager, JwtManager>();
        services.AddScoped<IImageManager, ImageManager>();
        
        // Db connection config --------------------------------------------------------------------
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("PgSQLConnection"));
            // option.UseSqlite(configuration.GetConnectionString("SQLiteConnection"));
        });
        
        // Db initializer config -------------------------------------------------------------------
        ApplicationDbContextInitializer
            .Init(services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>());
        
        // MinIO config ---------------------------------------------------------------
        services.AddMinio(options =>
        {
            options.Endpoint = configuration.GetSection("MinIO:Endpoint").Value!;
            options.AccessKey = configuration.GetSection("MinIO:AccessKey").Value!;
            options.SecretKey = configuration.GetSection("MinIO:SecretKey").Value!;
        });
        
        return services;
    }
}