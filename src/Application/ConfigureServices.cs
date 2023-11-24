using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Mediator config ------------------------------------------------------------
        services.AddMediatR(cnf => 
            cnf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // Mapster mapper config ------------------------------------------------------
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        
        return services;
    }
}