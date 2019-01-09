using System;
using SmallWorld.Database.Entities;

namespace SmallWorld.Auth
{
    public class AdminPermissions : IPermissions
    {
        public bool CreateWorld() => false;
        public bool CreateAccount() => true;

        public bool AccessWorld(World world) => true;
        public bool AccessAccount(Account acc) => true;

        public bool ModifyWorld(World world) => true;
        public bool ModifyAccount(Account acc) => true;

        public bool DeleteWorld(World world) => true;
        public bool DeleteAccount(Account acc) => true;
    }

    public class AdminSession : Session
    {
        public override IPermissions GetPermissions(IServiceProvider services)
        {
            return new AdminPermissions();
        }

        public override object Serialize(IServiceProvider services)
        {
            return new {
                token = Token,
            };
        }
    }
}