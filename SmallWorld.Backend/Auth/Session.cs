using System;

namespace SmallWorld.Auth
{
    public abstract class Session
    {
        public static readonly TimeSpan MaximumAge = TimeSpan.FromMinutes(30);

        public Guid Token { get; }

        public DateTime LastActivity { get; private set; }
        public bool IsExpired => DateTime.UtcNow - LastActivity > MaximumAge;

        protected Session()
        {
            Token = Database.Token.Generate();
            Update();
        }

        public abstract IPermissions GetPermissions(IServiceProvider services);

        public abstract object Serialize(IServiceProvider services);

        public void Update()
        {
            LastActivity = DateTime.UtcNow;
        }
    }
}
