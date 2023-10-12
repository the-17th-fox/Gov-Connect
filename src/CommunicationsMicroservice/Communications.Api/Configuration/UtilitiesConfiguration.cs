using Communications.Application;
using Communications.Application.Utilities;
using Communications.Application.ViewModels;
using Communications.Application.ViewModels.CiviliansInfoConsistency;
using Hangfire;
using SharedLib.ElasticSearch.Extensions;
using SharedLib.Kafka.Configurations;
using SharedLib.Redis.Configurations;
using System.Reflection;

namespace Communications.Api.Configuration;

internal static class UtilitiesConfiguration
{
    internal static IServiceCollection ConfigureUtilities(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(opt =>
        {
            opt.AddMaps(Assembly.GetExecutingAssembly());

            opt.AddProfile<ApplicationMapperProfile>();
        });

        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(typeof(HandlerBase<>).Assembly);
        });

        services.AddHangfireServer();

        services.ConfigureKafkaConsumers(configuration.GetConnectionString("KafkaBootstrapServers"));

        services.AddSignalR();

        services.ConfigureElasticSearchService(configuration);

        services.ConfigureRedisCaching(configuration);

        return services;
    }

    private static IServiceCollection ConfigureKafkaConsumers(this IServiceCollection services, string bootstrapServers)
    {
        services.AddKafkaConsumer<string, CivilianInfoViewModel>(opt =>
        {
            opt.Topic = "civilians-info-in-reports-update";
            opt.GroupId = "civilians-info-in-reports-update-group";
            opt.BootstrapServers = bootstrapServers;
            opt.AllowAutoCreateTopics = true;
        });

        return services;
    }

    private static IServiceCollection ConfigureElasticSearchService(this IServiceCollection services, IConfiguration configuration)
    {
        var uri = configuration.GetConnectionString("ElasticSearchNode");
        services.ConfigureElasticSearchClient(uri);

        var elasticSearchSection = configuration.GetSection("ElasticSearchConfiguration");
        services.Configure<ElasticSearchIndexesOptions>(elasticSearchSection);

        return services;
    }

    private static IServiceCollection ConfigureRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedis(configuration.GetConnectionString("RedisService"));

        var redisSection = configuration.GetSection("RedisServiceConfiguration");

        services.AddRedisConfiguration(opt =>
        {
            opt.IsEnabled = redisSection.GetValue<bool>("IsEnabled");
            opt.DefaultTTLSeconds = redisSection.GetValue<short>("DefaultTTLSeconds");
        });

        return services;
    }
}