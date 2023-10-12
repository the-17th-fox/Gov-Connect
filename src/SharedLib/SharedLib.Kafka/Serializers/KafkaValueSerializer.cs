using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace SharedLib.Kafka.Serializers;

internal class KafkaValueSerializer<TValue> : ISerializer<TValue>
{
    public byte[] Serialize(TValue data, SerializationContext context)
    {
        if (typeof(TValue) == typeof(Null))
        {
            return null!;
        }

        if (typeof(TValue) == typeof(Ignore))
        {
            throw new NotSupportedException();
        }

        var json = JsonConvert.SerializeObject(data);

        return Encoding.UTF8.GetBytes(json);
    }
}