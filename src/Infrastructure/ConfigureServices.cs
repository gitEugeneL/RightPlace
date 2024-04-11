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
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IAdvertRepository, AdvertRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<ITypeRepository, TypeRepository>()
            .AddScoped<IAddressRepository, AddressRepository>()
            .AddScoped<IInformationRepository, InformationRepository>()
            .AddScoped<IImageRepository, ImageRepository>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IJwtManager, JwtManager>()
            .AddScoped<IImageManager, ImageManager>();
        
        /*** Db connection config ***/
        services.AddDbContext<ApplicationDbContext>(option =>
            option.UseNpgsql(configuration.GetConnectionString("PgSQLConnection")));
            // option.UseSqlite(configuration.GetConnectionString("SQLiteConnection")));
        
        /*** Db initializer config ***/
        ApplicationDbContextInitializer
            .Init(services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>());
        
        /*** MinIO config ***/
        services.AddMinio(options =>
        {
            options.Endpoint = configuration.GetSection("MinIO:Endpoint").Value!;
            options.AccessKey = configuration.GetSection("MinIO:AccessKey").Value!;
            options.SecretKey = configuration.GetSection("MinIO:SecretKey").Value!;
        });
        
        return services;
    }
}