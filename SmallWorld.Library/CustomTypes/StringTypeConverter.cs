using System;
using Newtonsoft.Json;

namespace SmallWorld.Library.CustomTypes
{
    public class StringTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(StringType).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((StringType)value).Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            return Activator.CreateInstance(objectType, str);
        }
    }
}
