using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateStringsField : RequiredField
    {
        private readonly Func<string, bool> _predicate;

        public PredicateStringsField(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate) 
            : base(fieldName, minAmount, maxAmount)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(val => _predicate(val));
        }
    }
}