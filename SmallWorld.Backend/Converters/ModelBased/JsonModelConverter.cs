using System;
using Newtonsoft.Json;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Converters.ModelBased
{
    public class JsonModelConverter : JsonConverter
    {
        private readonly IEntryRepository context;

        public JsonModelConverter(IEntryRepository context)
        {
            this.context = context;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsSubclassOf(typeof(JsonModel)))
                return false;

            return ModelType.GetForType(objectType) != null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var type = ModelType.GetForType(value.GetType());
            var model = type.Instantiate();

            model.Load(value, context);

            serializer.Serialize(writer, model);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var modelType = ModelType.GetForType(objectType);

            var model = (JsonModel) serializer.Deserialize(reader, modelType.Type);

            return model.GetValue();
        }
    }
}
