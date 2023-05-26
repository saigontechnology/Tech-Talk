using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;

namespace TStore.Shared.Serdes
{
    // [Important] custom JSON serializer, deserializer without schema registry
    public class SimpleJsonSerdes<T> : ISerializer<T>, IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull) return default;

            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            if (data == null)
            {
                return null;
            }

            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}
