using System;

namespace Validation
{
    public abstract class PredicateField<T> : BasicField
    {
        protected readonly Func<T, bool> SatisfiesPredicate;

        protected PredicateField(string fieldName, int minAmount, int maxAmount, Func<T, bool> predicate, bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            SatisfiesPredicate = predicate;
        }
    }
}