using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Auth
{
    public class AccountPermissions : IPermissions
    {
        private readonly IServiceProvider services;
        private readonly Account account;

        public AccountPermissions(Account account, IServiceProvider services)
        {
            this.services = services;
            this.account = account;
        }

        public bool CreateWorld() => true;
        public bool CreateAccount() => false;

        private bool IsOwner(World world)
        {
            services.GetService<IEntryRepository>().Entry(world)
                .LoadRelations(w => w.Account);

            return world.Account.Id == account.Id;
        }

        public bool AccessWorld(World world) => IsOwner(world);
        public bool AccessAccount(Account acc) => acc == account;

        public bool ModifyWorld(World world) => IsOwner(world);
        public bool ModifyAccount(Account acc) => acc == account;

        public bool DeleteWorld(World world) => IsOwner(world);

        public bool DeleteAccount(Account acc) => acc == account;
    }

    public class AccountSession : Session
    {
        public Guid AccountId { get; }

        public AccountSession(Guid accountId)
        {
            AccountId = accountId;
        }

        public override IPermissions GetPermissions(IServiceProvider services)
        {
            return new AccountPermissions(GetAccount(services), services);
        }

        public override object Serialize(IServiceProvider services)
        {
            return new {
                token = Token,
                account = GetAccount(services),
            };
        }

        public Account GetAccount(IServiceProvider services)
        {
            var accounts = services.GetService<IAccountRepository>();
            accounts.Find(AccountId, out var acc);
            return acc;
        }
    }
    //    public class AccountSession : Session
    //    {
    //        public Account Account => HostingApplication.Context.Accounts.SingleOrDefault(a => a.Guid == AccountId);
    //        public Guid AccountId { get; }
    //
    //        public AccountSession(Account account)
    //        {
    //            AccountId = account.Guid;
    //        }
    //
    //        public override object Serialize()
    //        {
    //            return new {
    //                token = Token,
    //                account = Account
    //            };
    //        }
    //
    //        private bool CanAccess(IContext context, object o)
    //        {
    //            switch (o)
    //            {
    //                case World world:
    //                    context.Entry(world).LoadRelations(w => w.Account);
    //                    return world.Account.Guid == AccountId;
    //
    //                case Account account:
    //                    return account.Guid == AccountId;
    //
    //                default:
    //                    return false;
    //            }
    //        }
    //
    //        protected override bool CanCreate(Type t)
    //        {
    //            return t == typeof(World);
    //        }
    //
    //        protected override bool CanRead(IContext context, object o)
    //        {
    //            return CanAccess(context, o);
    //        }
    //
    //        protected override bool CanUpdate(IContext context, object o)
    //        {
    //            return CanAccess(context, o);
    //        }
    //
    //        protected override bool CanDelete(IContext context, object o)
    //        {
    //            return CanAccess(context, o);
    //        }
    //    }
}