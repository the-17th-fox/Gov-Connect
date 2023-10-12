namespace SharedLib.Kafka.Interfaces;

public interface IKafkaProducer<in TKey, in TValue>
{
    public Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
}