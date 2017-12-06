using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateStringsField : IntegersField
    {
        private readonly Func<string, bool> _predicate;

        public PredicateStringsField(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate) 
            : base(fieldName, minAmount, maxAmount)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= MinAmount &&
                   field.Count <= MaxAmount &&
                   field.All(val => _predicate(val));
        }
    }
}