using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    public class PredicateStringsField : PredicateField<string>
    {
        public PredicateStringsField(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, predicate, optional)
        { }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(value => (value == "" && Optional) || SatisfiesPredicate(value));
        }
    }
}