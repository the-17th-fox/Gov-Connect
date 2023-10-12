using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace SharedLib.Kafka.Serializers;

internal class KafkaValueDeserializer<TValue> : IDeserializer<TValue>
{
    public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (typeof(TValue) == typeof(Ignore))
        {
            return default!;
        }

        if (typeof(TValue) == typeof(Null))
        {
            if (data.Length > 0 || !isNull)
            {
                throw new ArgumentException("The type is set to Null, but data isn't null.");
            }

            return default!;
        }

        var encodedData = Encoding.UTF8.GetString(data);

        return JsonConvert.DeserializeObject<TValue>(encodedData)!;
    }
}