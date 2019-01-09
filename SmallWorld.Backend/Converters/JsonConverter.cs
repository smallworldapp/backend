using System;
using Newtonsoft.Json;

namespace SmallWorld.Converters
{
    public abstract class JsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        protected abstract T ReadJson(JsonReader reader, JsonSerializer serializer);
        protected abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = ReadJson(reader, serializer);
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WriteJson(writer, (T) value, serializer);
        }
    }
}