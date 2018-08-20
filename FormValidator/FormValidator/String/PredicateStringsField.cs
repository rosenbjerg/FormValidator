using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    public class PredicateStringsField : BasicField
    {
        private readonly Func<string, bool> _predicate;

        public PredicateStringsField(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, optional)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(val => _predicate(val));
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(val => _predicate(val));
        }
    }
}