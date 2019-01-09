namespace SmallWorld.Database.Updates
{
    public interface IUpdate
    {
        string Identifier { get; }
        void Apply(SmallWorldContext context);
    }

    public abstract class Update : IUpdate
    {
        public string Identifier { get; }
        public abstract void Apply(SmallWorldContext context);

        protected Update()
        {
            Identifier = GetType().Name;
        }
    }
}
