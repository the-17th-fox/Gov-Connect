namespace SharedLib.Kafka.Interfaces;

public interface IKafkaConsumer<TValue>
{
    public void Consume(out TValue consumerValue);
}