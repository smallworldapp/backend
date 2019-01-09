using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Worlds
{
    public class ApplicationQuestionConverter : EntityConverter<ApplicationQuestion>
    {
        public string question
        {
            get => Value.Question;
            set => Value.Question = value;
        }

        public string subtext
        {
            get => Value.Subtext;
            set => Value.Subtext = value;
        }

        public string answer
        {
            get => Value.Answer;
            set => Value.Answer = value;
        }
    }

    //public class ApplicationQuestionConverter : QuickJsonConverter<ApplicationQuestion>
    //{
    //    public ApplicationQuestionConverter()
    //    {
    //        Default("question", a => a.Question);
    //        Default("subtext", a => a.Subtext);
    //        Default("answer", a => a.Answer);

    //        Default("id", a => a.Guid);
    //    }
    //}
}