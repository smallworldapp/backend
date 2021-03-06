﻿using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Members
{
    [TypeValidator]
    public class PairingTypeValidator : Validator<PairingType>
    {
        protected override bool Validate(IValidationTarget<PairingType> target)
        {
            if (target.Value == PairingType.ERROR)
                return target.Error("Invalid pair outcome");

            return true;
        }
    }
}
