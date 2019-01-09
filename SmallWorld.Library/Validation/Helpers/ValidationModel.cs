using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Helpers
{
    public enum Effectiveness
    {
        Effective,
        Uncertain,
        Ineffective,
    }

    public class ValidationModel<T>
    {
        private readonly IList<TypeValidator<T>> typeValidators = new List<TypeValidator<T>>();
        private IList<MemberValidator<T>> memberValidators = new List<MemberValidator<T>>();

        private bool? isEffective;
        private bool isTrim;

        public bool IsEffective => isEffective ?? (isEffective = GetEffectiveness()).Value;

        private bool recursiveCheck;
        private bool GetEffectiveness()
        {
            if (recursiveCheck)
                return false;

            if (typeValidators.Any())
                return true;

            if (!memberValidators.Any())
                return false;

            if (memberValidators.Any(v => v.HasValidators()))
                return true;

            recursiveCheck = true;
            return memberValidators.Any(v => v.IsEffective);
        }

        public void Trim()
        {
            if (isTrim) return;
            isTrim = true;

            isEffective = null;
            recursiveCheck = false;

            memberValidators = memberValidators
                .Where(v => v.IsEffective)
                .ToList();

            foreach (var member in memberValidators)
                member.Trim();
        }

        public void AddValidator(TypeValidator<T> validator)
        {
            Debug.Assert(!isEffective.HasValue);
            typeValidators.Add(validator);
        }

        public void AddValidator(MemberValidator<T> validator)
        {
            Debug.Assert(!isEffective.HasValue);
            memberValidators.Add(validator);
        }

        public void Validate(IValidationProvider validation, IValidationTarget<T> target)
        {
            if (target.Value == null)
                return;

            foreach (var member in memberValidators)
            {
                member.Validate(validation, target);
                if (!target.Continue) return;
            }

            foreach (var type in typeValidators)
            {
                type.Validate(validation, target);
                if (!target.Continue) return;
            }
        }
    }
}