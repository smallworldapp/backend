using SmallWorld.Database.Entities;

namespace SmallWorld.Auth
{
    public interface IPermissions
    {
        bool CreateWorld();
        bool CreateAccount();

        bool AccessWorld(World world);
        bool AccessAccount(Account acc);

        bool ModifyWorld(World world);
        bool ModifyAccount(Account acc);

        bool DeleteWorld(World world);
        bool DeleteAccount(Account acc);
    }
}
