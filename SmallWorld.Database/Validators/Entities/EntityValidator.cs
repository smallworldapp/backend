using SmallWorld.Database.Entities;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities
{
    [TypeValidator]
    public class EntityValidator : Validator<BaseEntity>
    {
        private readonly IEntryRepository entries;

        public EntityValidator(IEntryRepository entries)
        {
            this.entries = entries;
        }

        protected override bool Validate(IValidationTarget<BaseEntity> target)
        {
            var entry = entries.Entry(target.Value);
            if (entry.State == EntityState.Unchanged)
                target.Continue = false;

            return true;
        }
    }
}
