using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    public class PredicateIntegersField : BasicField
    {
        private readonly Func<int, bool> _predicate;

        public PredicateIntegersField(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate,
            bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            _predicate = predicate;
        }

        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return field.All(val => int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return field.All(val => int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }
    }
}