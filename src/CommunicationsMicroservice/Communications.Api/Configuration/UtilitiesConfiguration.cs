using Communications.Application;
using Communications.Application.Utilities;
using Communications.Application.ViewModels;
using Communications.Application.ViewModels.CiviliansInfoConsistency;
using Hangfire;
using SharedLib.ElasticSearch.Extensions;
using SharedLib.Kafka.Configurations;
using System.Reflection;

namespace Communications.Api.Configuration;

internal static class UtilitiesConfiguration
{
    internal static IServiceCollection ConfigureUtilities(this IServiceCollection services, ConfigurationManager configuration)
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

    private static IServiceCollection ConfigureElasticSearchService(this IServiceCollection services, ConfigurationManager configuration)
    {
        var uri = configuration.GetConnectionString("ElasticSearchNode");
        services.ConfigureElasticSearchClient(uri);

        var elasticSearchSection = configuration.GetSection("ElasticSearch");
        services.Configure<ElasticSearchIndexesOptions>(elasticSearchSection);

        return services;
    }
}