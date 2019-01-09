using SmallWorld.Database.Entities;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Members
{
    [TypeValidator]
    public class PairValidator : Validator<Pair>
    {
        private readonly IEntryRepository entries;

        public PairValidator(IEntryRepository entries)
        {
            this.entries = entries;
        }

        protected override bool Validate(IValidationTarget<Pair> target)
        {
            entries.Entry(target.Value)
                .LoadRelations(p => p.World)
                .LoadRelations(p => p.Pairing.World);

            if (target.Value.World != target.Value.Pairing.World)
                return target.Error("World and Pairing.World mismatch");

            return true;
        }
    }
}
