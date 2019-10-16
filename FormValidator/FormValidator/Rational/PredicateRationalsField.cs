using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    public class PredicateRationalsField : PredicateField<double>
    {
        public PredicateRationalsField(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, predicate, optional)
        { }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(val => 
                (val == "" && Optional) || double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                SatisfiesPredicate(parsed));
        }
    }
}