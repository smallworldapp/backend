using System.Collections.Generic;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Worlds
{
    public class DescriptionConverter : EntityConverter<Description>
    {
        public override void Load(IEntry<Description> entry, IEntryRepository context)
        {
            entry.LoadRelations(d => d.Faq);
        }

        public string goals
        {
            get => Value.Goals;
            set => Value.Goals = value;
        }

        public string introduction
        {
            get => Value.Introduction;
            set => Value.Introduction = value;
        }

        public IEnumerable<FaqItem> faq
        {
            get => Value.Faq;
            set => Value.Faq = new HashSet<FaqItem>(value);
        }
    }

    //public class DescriptionConverter : QuickJsonConverter<Description>
    //{
    //    private readonly SmallWorldContext context;

    //    public DescriptionConverter(SmallWorldContext context)
    //    {
    //        this.context = context;

    //        Default("goals", a => a.Goals);
    //        Default("introduction", a => a.Introduction);

    //        Default("faq", a => a.Faq);
    //    }

    //    protected override void WriteJson(JsonWriter writer, Description value, JsonSerializer serializer)
    //    {
    //        context.Entry(value).LoadRelations(d => d.Faq);

    //        base.WriteJson(writer, value, serializer);
    //    }
    //}
}