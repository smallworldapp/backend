using System.Collections.Generic;
using Newtonsoft.Json;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Converters.Worlds
{
    public class ApplicationConverter : JsonConverter<Application>
    {
        private readonly IEntryRepository context;

        public ApplicationConverter(IEntryRepository context)
        {
            this.context = context;
        }

        protected override Application ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var list = serializer.Deserialize<List<ApplicationQuestion>>(reader);
            return new Application { Questions = list };
        }

        protected override void WriteJson(JsonWriter writer, Application value, JsonSerializer serializer)
        {
            context.Entry(value).LoadRelations(a => a.Questions);

            writer.WriteStartArray();

            foreach (var item in value.Questions)
                serializer.Serialize(writer, item);

            writer.WriteEnd();
        }
    }
}