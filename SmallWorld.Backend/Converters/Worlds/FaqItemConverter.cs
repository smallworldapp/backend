using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Worlds
{
    public class FaqItemConverter : EntityConverter<FaqItem>
    {
        public string question
        {
            get => Value.Question;
            set => Value.Question = value;
        }

        public string answer
        {
            get => Value.Answer;
            set => Value.Answer = value;
        }
    }

    //public class FaqItemSerializer : QuickJsonConverter<FaqItem>
    //{
    //    public FaqItemSerializer()
    //    {
    //        Default("question", a => a.Question);
    //        Default("answer", a => a.Answer);
    //    }
    //}
}