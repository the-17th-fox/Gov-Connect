using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SharedLib.Kafka.Configurations;
using SharedLib.Kafka.Interfaces;

namespace SharedLib.Kafka;

public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>, IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly string _topic;

    public KafkaProducer(IProducer<TKey, TValue> producer, IOptions<KafkaProducerConfiguration> producerConfiguration)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _topic = producerConfiguration.Value.Topic 
                 ?? throw new ArgumentNullException(nameof(producerConfiguration.Value.Topic));
    }

    public async Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken)
    {
        var message = new Message<TKey, TValue>()
        {
            Key = key,
            Value = value
        };

        await _producer.ProduceAsync(_topic, message, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}