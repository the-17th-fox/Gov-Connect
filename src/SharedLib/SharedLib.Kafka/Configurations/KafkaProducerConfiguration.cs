using Confluent.Kafka;

namespace SharedLib.Kafka.Configurations;

public class KafkaProducerConfiguration : ProducerConfig
{
    public string Topic { get; set; } = string.Empty;
}