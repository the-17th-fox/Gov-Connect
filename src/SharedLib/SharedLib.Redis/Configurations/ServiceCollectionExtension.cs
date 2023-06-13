using Microsoft.Extensions.DependencyInjection;
using SharedLib.Redis.Interfaces;
using StackExchange.Redis;

namespace SharedLib.Redis.Configurations;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));

        services.AddScoped<IRedisService, IRedisService>();
        
        return services;
    }

    public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, 
        Action<RedisCacheConfiguration> cacheConfiguration)
    {
        services.Configure(cacheConfiguration);

        return services;
    }
}