using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedLib.Kafka.Interfaces;
using SharedLib.Kafka.Serializers;

namespace SharedLib.Kafka.Configurations;

public static class KafkaServicesConfiguration
{
    public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services,
        Action<KafkaConsumerConfiguration> consumerConfiguration)
    {
        services.Configure(consumerConfiguration);
        services.AddScoped<IKafkaConsumer<TValue>, KafkaConsumer<TKey, TValue>>();

        return services;
    }

    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
        Action<KafkaProducerConfiguration> producerConfiguration)
    {
        services.Configure(producerConfiguration);
        services.AddApacheKafkaProducer<TKey, TValue>();
        services.AddSingleton<IKafkaProducer<TKey, TValue>, KafkaProducer<TKey, TValue>>();

        return services;
    }

    private static IServiceCollection AddApacheKafkaProducer<TKey, TValue>(this IServiceCollection services)
    {
        services.AddSingleton(s =>
        {
            var configuration = s.GetRequiredService<IOptions<KafkaProducerConfiguration>>();

            if (configuration.Value == null)
            {
                throw new ArgumentException(nameof(configuration.Value));
            }

            var builder = new ProducerBuilder<TKey, TValue>(configuration.Value)
                .SetValueSerializer(new KafkaValueSerializer<TValue>());

            return builder.Build();
        });

        return services;
    }
}