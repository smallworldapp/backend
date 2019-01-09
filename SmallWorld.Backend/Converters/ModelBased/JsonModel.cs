using Newtonsoft.Json;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Converters.ModelBased
{
    public abstract class JsonModel
    {
        public abstract object GetValue();

        public abstract void Load(object value, IEntryRepository context);
    }

    public abstract class JsonModel<T> : JsonModel where T : class, new()
    {
        [JsonIgnore]
        protected T Value { get; private set; } = new T();

        public sealed override object GetValue() => Value;

        public sealed override void Load(object value, IEntryRepository context)
        {
            Value = (T) value;
            Load(context.Entry(Value), context);
        }

        public virtual void Load(IEntry<T> entry, IEntryRepository context) { }
    }
}
