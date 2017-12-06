using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateRationalsField : IntegersField
    {
        private readonly Func<double, bool> _predicate;

        public PredicateRationalsField(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate) 
            : base(fieldName, minAmount, maxAmount)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= MinAmount &&
                   field.Count <= MaxAmount &&
                   field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }
    }
}