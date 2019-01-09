using SmallWorld.Database.Entities;

namespace SmallWorld.Models.Emailing.Abstractions
{
    public interface IEmailTemplateBase
    {
    }

    public interface IEmailTemplate : IEmailTemplateBase
    {
        Email Create();
    }

    public interface IEmailTemplate<in T1> : IEmailTemplateBase
    {
        Email Create(T1 t1);
    }

    public interface IEmailTemplate<in T1, in T2> : IEmailTemplateBase
    {
        Email Create(T1 t1, T2 t2);
    }

    public interface IEmailTemplate<in T1, in T2, in T3> : IEmailTemplateBase
    {
        Email Create(T1 t1, T2 t2, T3 t3);
    }
}
