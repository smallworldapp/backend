using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Accounts
{
    public class AccountConverter : EntityConverter<Account>
    {
        public Name name
        {
            get => Value.Name;
            set => Value.Name = value;
        }

        public EmailAddress email
        {
            get => Value.Email;
            set => Value.Email = value;
        }

        public AccountType type
        {
            get => Value.Type;
            set => Value.Type = value;
        }

        public AccountStatus status
        {
            get => Value.Status;
            set => Value.Status = value;
        }
    }

    //public class AccountConverter : QuickJsonConverter<Account>
    //{
    //    private readonly SmallWorldContext context;

    //    public AccountConverter(SmallWorldContext context)
    //    {
    //        this.context = context;

    //        Default("id", a => a.Guid);

    //        Default("name", a => a.Name);
    //        Default("email", a => a.Email);

    //        Default("status", a => a.Status);

    //        //Default("worlds", a => a.Worlds);
    //    }

    //    protected override void WriteJson(JsonWriter writer, Account value, JsonSerializer serializer)
    //    {
    //        var entry = context.Entry(value);

    //        entry.LoadRelations(a => a.Worlds);

    //        base.WriteJson(writer, value, serializer);
    //    }
    //}
}
