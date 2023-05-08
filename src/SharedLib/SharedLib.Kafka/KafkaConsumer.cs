using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SharedLib.Kafka.Configurations;
using SharedLib.Kafka.Interfaces;
using SharedLib.Kafka.Serializers;

namespace SharedLib.Kafka;

public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TValue>
{
    private readonly KafkaConsumerConfiguration _consumerConfiguration;

    public KafkaConsumer(IOptions<KafkaConsumerConfiguration> consumerConfiguration)
    {
        _consumerConfiguration = consumerConfiguration.Value
                                 ?? throw new ArgumentNullException(nameof(consumerConfiguration));
    }

    public void Consume(out TValue value)
    {
        using var consumer = GetConsumerBuilder().Build();
        consumer.Subscribe(_consumerConfiguration.Topic);

        var result = consumer.Consume(TimeSpan.FromMinutes(1));

        if (result == null)
        {
            value = default!;
            return;
        }

        consumer.Commit(result);
        consumer.StoreOffset(result);

        value = result.Message.Value;
    }

    private ConsumerBuilder<TKey, TValue> GetConsumerBuilder()
    {
        return new ConsumerBuilder<TKey, TValue>(_consumerConfiguration)
            .SetValueDeserializer(new KafkaValueDeserializer<TValue>());
    }
}