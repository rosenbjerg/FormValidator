using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    public class PredicateIntegersField : PredicateField<int>
    {
        public PredicateIntegersField(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate,
            bool optional = false) : base(fieldName, minAmount, maxAmount, predicate, optional)
        { }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(val =>
                (val == "" && Optional) || int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                SatisfiesPredicate(parsed));
        }
    }
}