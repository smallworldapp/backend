using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Members
{
    [TypeValidator]
    public class MemberValidator : Validator<Member>
    {
        private readonly IEntryRepository entries;
        private readonly IWorldRepository worlds;

        public MemberValidator(IEntryRepository entries, IWorldRepository worlds)
        {
            this.entries = entries;
            this.worlds = worlds;
        }

        protected override bool Validate(IValidationTarget<Member> target)
        {
            entries.Entry(target.Value)
                .LoadRelations(m => m.World);

            var members = worlds.Members(target.Value.World);
            if (members.Find(target.Value.Email, out var same) && same != target.Value)
                return target.Error("An account already exists with that email");

            return true;
        }
    }
}
