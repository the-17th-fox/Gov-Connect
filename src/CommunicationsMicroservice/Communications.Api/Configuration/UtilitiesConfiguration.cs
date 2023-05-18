using Communications.Application;
using Communications.Application.AutoMapper;
using Communications.Application.AutoMapper.CiviliansInfoConsistency;
using Hangfire;
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

}