using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateIntegersField : IntegersField
    {
        private readonly Func<int, bool> _predicate;

        public PredicateIntegersField(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate) : base(fieldName, minAmount, maxAmount)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= MinAmount &&
                   field.Count <= MaxAmount &&
                   field.All(val => int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }
    }
}