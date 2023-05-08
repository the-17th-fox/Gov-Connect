using Confluent.Kafka;

namespace SharedLib.Kafka.Configurations;

public class KafkaConsumerConfiguration : ConsumerConfig
{
    public string Topic { get; set; } = string.Empty;

	public KafkaConsumerConfiguration()
    {
        AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        EnableAutoCommit = true;
        EnableAutoOffsetStore = false;
    }
}