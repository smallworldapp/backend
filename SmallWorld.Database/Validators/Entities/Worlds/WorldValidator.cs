using System.Diagnostics;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Worlds
{
    [TypeValidator]
    public class WorldValidator : Validator<World>
    {
        private readonly IWorldRepository worlds;

        public WorldValidator(IWorldRepository worlds)
        {
            this.worlds = worlds;
        }

        protected override bool Validate(IValidationTarget<World> target)
        {
            Debug.Assert(target.Value.Account != null, "Validation or navigation failure");

            if (target.Value.Account.Type == AccountType.Research && target.Value.Privacy != WorldPrivacy.InviteOnly)
                return target.Error("Invalid world privacy");

            if (worlds.Find(target.Value.Identifier, out var same) && same != target.Value)
                return target.Error("Duplicate identifier");

            return true;
        }
    }
}
